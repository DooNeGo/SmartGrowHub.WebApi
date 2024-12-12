using SmartGrowHub.Shared.Tokens;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class RefreshTokensEndpoint
{
    // public static Task<IResult> RefreshTokens(
    //     RefreshTokensRequestDto requestDto, IAuthService sessionService,
    //     ILogger<RefreshTokensEndpoint> logger, CancellationToken cancellationToken) =>
    //     (from request in requestDto.TryToDomain().ToEff()
    //      from response in sessionService.RefreshTokens(request, cancellationToken)
    //      select response)
    //         .RunAsync()
    //         .Map(effect => effect.Match(
    //             Succ: response => Ok(response.ToDto()),
    //             Fail: error => HandleError(logger, error)));
}
