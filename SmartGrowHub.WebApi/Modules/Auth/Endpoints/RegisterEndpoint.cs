using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class RegisterEndpoint
{
    public static Task<IResult> Register(IAuthService authService, RegisterRequestDto request,
        ILogger<RegisterEndpoint> logger, CancellationToken cancellationToken) =>
        request.TryToDomain()
            .Map(request => authService.RegisterAsync(request, cancellationToken))
            .Match(
                Succ: task => task.Match(
                    Right: _ => Created(),
                    Left: exception => HandleException(logger, exception)),
                Fail: error => BadRequest(error.Message).AsTask());
}