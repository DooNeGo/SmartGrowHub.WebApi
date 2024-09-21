using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class RegisterEndpoint
{
    public static Task<IResult> Register(IAuthService authService, RegisterRequestDto requestDto,
        ILogger<RegisterEndpoint> logger, CancellationToken cancellationToken) =>
        (from request in requestDto.TryToDomain().ToEff()
         from response in authService.RegisterAsync(request, cancellationToken)
         select response)
        .RunAsync()
        .Map(effect => effect.Match(
            Succ: _ => Created(),
            Fail: exception => HandleException(logger, exception)));
}