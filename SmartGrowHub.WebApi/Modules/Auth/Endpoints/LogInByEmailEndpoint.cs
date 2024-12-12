using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Auth;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInByEmailEndpoint
{
    public static Task<IResult> LogIn(LogInByEmailRequestDto requestDto, OtpLoginUseCase useCase,
        ILogger<LogInByEmailEndpoint> logger, CancellationToken cancellationToken) =>
        (from email in EmailAddress.From(requestDto.EmailAddress).ToEff()
            from _ in useCase.SendCodeToEmail(email, cancellationToken)
            select unit)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: _ => Ok(),
            Fail: error => HandleError(logger, error)));
}