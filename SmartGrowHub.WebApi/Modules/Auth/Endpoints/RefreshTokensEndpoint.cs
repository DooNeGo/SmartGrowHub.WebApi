using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.WebApi.Modules.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class RefreshTokensEndpoint
{
    public static ValueTask<IResult> RefreshTokens(
        RefreshTokensRequest requestDto, RefreshTokensUseCase useCase,
        ILogger<RefreshTokensEndpoint> logger, CancellationToken cancellationToken) => (
            from oldToken in UlidFp.From(requestDto.RefreshToken)
                .MapFail(error => Error.New("Invalid refresh token format", error))
                .ToIO()
            from response in useCase.RefreshTokens(oldToken, cancellationToken)
            select response)
        .RunSafeAsync()
        .Map(effect => effect.Match(
            Succ: response => Ok(Result<AuthTokensDto>.Success(response.ToDto())),
            Fail: error => HandleError(logger, error)));
}

public static class UlidFp
{
    public static Fin<Ulid> From(string ulid) =>
        Ulid.TryParse(ulid, out Ulid result)
            ? result : Error.New("Invalid ulid");
}
