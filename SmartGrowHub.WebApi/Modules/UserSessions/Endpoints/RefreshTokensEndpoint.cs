using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.UserSessions.Endpoints;

public sealed class RefreshTokensEndpoint
{
    public static Task<IResult> RefreshTokens(RefreshTokensRequestDto request,
        IUserSessionService sessionService, ILogger<RefreshTokensEndpoint> logger,
        CancellationToken cancellationToken) =>
        (from refreshToken in RefreshToken.Create(request.RefreshToken ?? "").ToEff()
         from authTokens in sessionService.RefreshTokensAsync(refreshToken, cancellationToken)
         select authTokens)
        .RunAsync()
        .Map(effect => effect.Match(
            Succ: tokens => Ok(new RefreshTokensResponseDto(tokens.AccessToken, tokens.RefreshToken)),
            Fail: exception => HandleException(logger, exception)));
}
