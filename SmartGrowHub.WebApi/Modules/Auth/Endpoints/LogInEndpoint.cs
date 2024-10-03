using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInEndpoint
{
    public static Task<IResult> LogIn(IAuthService authService, ILogger<LogInEndpoint> logger,
        LogInRequestDto requestDto, CancellationToken cancellationToken) =>
        (from request in requestDto.TryToDomain().ToEff()
         from response in authService.LogInAsync(request, cancellationToken)
         select response)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: response => Ok(response.ToDto()),
            Fail: exception => HandleException(logger, exception)));
}