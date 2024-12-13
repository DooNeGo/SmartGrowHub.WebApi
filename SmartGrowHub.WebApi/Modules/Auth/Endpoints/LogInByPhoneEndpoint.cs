using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Auth;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInByPhoneEndpoint
{
    public static Task<IResult> LogIn(LogInByPhoneRequest request, SendOtpToPhoneUseCase useCase,
        ILogger<LogInByEmailEndpoint> logger, CancellationToken cancellationToken) => (
            from phone in PhoneNumber.From(request.PhoneNumber).ToEff()
            from _ in useCase.SendCodeToPhone(phone, cancellationToken)
            select unit)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: _ => Ok(),
            Fail: error => HandleError(logger, error)));
}