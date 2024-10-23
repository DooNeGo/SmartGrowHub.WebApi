using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInEndpoint
{
    public static Task<IResult> LogIn(
        IAuthService authService, ILogger<LogInEndpoint> logger,
        LogInRequestDto requestDto, CancellationToken cancellationToken) =>
        LogIn(requestDto, authService, cancellationToken)
            .RunAsync()
            .Map(fin => fin.Match(
                Succ: response => Ok(response.ToDto()),
                Fail: error => HandleError(logger, error)));

    private static Eff<LogInResponse> LogIn(LogInRequestDto requestDto,
        IAuthService authService, CancellationToken cancellationToken) =>
        from request in requestDto.TryToDomain().ToEff()
        from response in authService.LogIn(request, cancellationToken)
        select response;
}