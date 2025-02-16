using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.WebApi.Modules.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class CheckOtpEndpoint
{
    public static Task<IResult> CheckOtp(CheckOtpRequest request, CheckOtpUseCase useCase,
        ILogger<CheckOtpEndpoint> logger,
        CancellationToken cancellationToken) => (
        from otp in NonEmptyString.From(request.OtpValue).ToEff()
        from result in useCase.CheckOtp(otp, cancellationToken)
        select result
    ).RunAsync().Map(fin => fin.Match(
        Succ: tokens => Ok(Result<AuthTokensDto>.Success(tokens.ToDto())),
        Fail: error => HandleError(logger, error)));
}