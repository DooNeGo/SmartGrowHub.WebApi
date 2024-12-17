using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInByEmailEndpoint
{
    public static Task<IResult> LogIn(LogInByEmailRequest request, SendOtpToEmailUseCase useCase,
        ILogger<LogInByEmailEndpoint> logger, CancellationToken cancellationToken) => (
            from email in EmailAddress.From(request.EmailAddress).ToEff()
            from _ in useCase.SendCodeToEmail(email, cancellationToken)
            select unit)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: _ => Ok(new Result(true, null, null)),
            Fail: error => HandleError(logger, error)));
}