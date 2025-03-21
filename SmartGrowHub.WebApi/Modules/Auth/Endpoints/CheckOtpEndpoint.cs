using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.WebApi.Modules.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class CheckOtpEndpoint
{
    public static ValueTask<IResult> CheckOtp(CheckOtpRequest request, CheckOtpUseCase useCase,
        ILogger<CheckOtpEndpoint> logger,
        CancellationToken cancellationToken) => (
            from otp in NonEmptyString.From(request.OtpValue).ToIO()
            from result in useCase.CheckOtp(otp, cancellationToken)
            select result)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            Succ: tokens => Ok(Result<AuthTokensDto>.Success(tokens.ToDto())),
            Fail: error => HandleError(logger, error)));
}