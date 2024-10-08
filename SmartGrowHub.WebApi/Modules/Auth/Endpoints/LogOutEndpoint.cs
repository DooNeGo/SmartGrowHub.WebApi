﻿using SmartGrowHub.Shared.Auth.Dto.LogOut;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

internal sealed class LogOutEndpoint
{
    public static Task<IResult> LogOut(IAuthService authService, ILogger<LogInEndpoint> logger,
        LogOutRequestDto requestDto, CancellationToken cancellationToken) =>
        (from request in Pure(requestDto.ToDomain())
         from response in authService.LogOutAsync(request, cancellationToken)
         select response)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: response => Ok(response),
            Fail: exception => HandleException(logger, exception)));
}
