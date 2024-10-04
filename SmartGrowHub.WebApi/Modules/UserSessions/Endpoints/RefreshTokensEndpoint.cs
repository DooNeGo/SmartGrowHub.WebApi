using SmartGrowHub.Shared.UserSessions.Dto;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.UserSessions.Endpoints;

public sealed class RefreshTokensEndpoint
{
    public static Task<IResult> RefreshTokens(RefreshTokensRequestDto requestDto,
        IUserSessionService sessionService, ILogger<RefreshTokensEndpoint> logger,
        CancellationToken cancellationToken) =>
        (from request in requestDto.TryToDomain().ToEff()
        from response in sessionService.RefreshTokensAsync(request.RefreshToken, cancellationToken)
        select response)
            .RunAsync()
            .Map(effect => effect.Match(
                Succ: tokens => Ok(new RefreshTokensResponseDto(tokens.ToDto())),
                Fail: exception => HandleException(logger, exception)));
}
