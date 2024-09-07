using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.InternalExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInEndpoint
{
    public static Task<IResult> LogIn(IAuthService authService, ILogger<LogInEndpoint> logger,
        LogInRequestDto request, CancellationToken cancellationToken) =>
        request.TryToDomain()
            .Map(request => authService.LogInAsync(request, cancellationToken))
            .Match(
                Succ: either => either.Match(
                    Right: response => Ok(response.ToDto()),
                    Left: exception => exception is InternalException internalException
                        ? HandleInternalException(logger, internalException)
                        : BadRequest(exception.Message)),
                Fail: error => BadRequest(error.Message).AsTask());
}