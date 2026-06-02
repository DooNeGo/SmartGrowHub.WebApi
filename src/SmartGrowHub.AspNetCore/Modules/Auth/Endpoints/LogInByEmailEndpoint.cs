using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

internal sealed class LogInByEmailEndpoint
{
    public static ValueTask<IResult> LogIn(LogInByEmailRequest request, SendOtpToEmailUseCase useCase,
        ILogger<LogInByEmailEndpoint> logger, CancellationToken cancellationToken) => (
            from email in EmailAddress.From(request.EmailAddress).ToIO()
            from _ in useCase.SendOtpToEmail(email)
            select unit)
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            Succ: _ => Ok(new Result(true, null, null)),
            Fail: error => HandleError(logger, error)));
}