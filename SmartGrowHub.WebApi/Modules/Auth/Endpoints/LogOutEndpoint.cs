using SmartGrowHub.Application.Services;
using SmartGrowHub.Shared.Auth.Dto.LogOut;
using SmartGrowHub.Shared.Auth.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

// TODO: Create SharedKernel instead of shared application

internal sealed class LogOutEndpoint
{
    public static Task<IResult> LogOut(
        IAuthService authService, ILogger<LogInEndpoint> logger,
        LogOutRequestDto requestDto, CancellationToken cancellationToken) =>
        (from request in requestDto.TryToDomain().ToEff()
         from response in authService.LogOut(request, cancellationToken)
         select response)
            .RunAsync()
            .Map(fin => fin.Match(
                Succ: response => Ok(response),
                Fail: error => HandleError(logger, error)));
}
