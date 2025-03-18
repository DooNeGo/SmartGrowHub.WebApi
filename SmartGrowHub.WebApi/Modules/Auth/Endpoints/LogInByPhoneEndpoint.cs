using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Auth;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class LogInByPhoneEndpoint
{
    public static ValueTask<IResult> LogIn(LogInByPhoneRequest request, SendOtpToPhoneUseCase useCase,
        ILogger<LogInByEmailEndpoint> logger, CancellationToken cancellationToken) => (
            from phone in PhoneNumber.From(request.PhoneNumber).ToIO()
            from _ in useCase.SendCodeToPhone(phone, cancellationToken)
            select unit)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            Succ: _ => Ok(),
            Fail: error => HandleError(logger, error)));
}