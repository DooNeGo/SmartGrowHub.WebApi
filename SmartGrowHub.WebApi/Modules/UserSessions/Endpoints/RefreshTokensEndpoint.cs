using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Shared.UserSessions.Dto;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.UserSessions.Endpoints;

public sealed class RefreshTokensEndpoint
{
    public static Task<IResult> RefreshTokens(
        RefreshTokensRequestDto request,
        IUserSessionService sessionService,
        ILogger<RefreshTokensEndpoint> logger,
        CancellationToken cancellationToken)
        => RefreshToken.Create(request.RefreshToken).Match(
            Succ: token => sessionService.RefreshTokensAsync(token, cancellationToken)
                .Match(
                    Right: tokens => Ok(response.ToDto()),
                    Left: exception => HandleException(logger, exception)),
            Fail: error => HandleException(logger, error.ToException()).AsTask());
}
