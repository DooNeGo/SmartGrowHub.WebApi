﻿using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

internal sealed class LogOutEndpoint
{
    // public static Task<IResult> LogOut(
    //     IAuthService authService, ILogger<LoginEndpoint> logger,
    //     LogOutRequestDto requestDto, CancellationToken cancellationToken) =>
    //     (from request in requestDto.TryToDomain().ToEff()
    //      from response in authService.LogOut(request, cancellationToken)
    //      select response)
    //         .RunAsync()
    //         .Map(fin => fin.Match(
    //             Succ: response => Ok(response),
    //             Fail: error => HandleError(logger, error)));
}
