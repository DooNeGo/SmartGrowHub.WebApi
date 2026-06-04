using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Auth;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

internal sealed class RequestOtpToPhoneEndpoint
{
    public static ValueTask<IResult> Request(LogInByPhoneRequest request, RequestOtpToPhoneUseCase useCase,
        ILogger<RequestOtpToEmailEndpoint> logger, CancellationToken cancellationToken) => (
            from phone in PhoneNumber.From(request.PhoneNumber).ToIO()
            from _ in useCase.RequestOtpToPhone(phone)
            select unit)
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            Succ: _ => Ok(),
            Fail: error => HandleError(logger, error)));
}