using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

internal sealed class VerifyOtpEndpoint
{
    public static ValueTask<IResult> Verify(VerifyOtpRequest request, VerifyOtpUseCase useCase,
        ILogger<VerifyOtpEndpoint> logger,
        CancellationToken cancellationToken) => (
            from otp in NonEmptyString.From(request.OtpValue).ToIO()
            from result in useCase.VerifyOtp(otp)
            select result)
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            Succ: tokens => Ok(Result<AuthTokensDto>.Success(tokens.ToDto())),
            Fail: error => HandleError(logger, error)));
}