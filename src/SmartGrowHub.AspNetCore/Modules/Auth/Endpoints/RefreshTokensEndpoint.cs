using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Common;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

internal sealed class RefreshTokensEndpoint
{
    public static ValueTask<IResult> RefreshTokens(
        RefreshTokensRequest requestDto, RefreshTokensUseCase useCase,
        ILogger<RefreshTokensEndpoint> logger, CancellationToken cancellationToken) => (
            from oldToken in NonEmptyString.From(requestDto.RefreshToken)
                .MapFail(error => Error.New("Invalid refresh token format", error))
                .ToIO()
            from response in useCase.RefreshTokens(oldToken, cancellationToken)
            select response)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            Succ: response => Ok(Result<AuthTokensDto>.Success(response.ToDto())),
            Fail: error => HandleError(logger, error)));
}
