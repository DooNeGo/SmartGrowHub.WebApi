using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class CheckOtpUseCase(
    IOtpRepository otpRepository,
    IUserRepository userRepository,
    IUserService userService,
    ITimeProvider timeProvider)
{
    public IO<AuthTokens> CheckOtp(NonEmptyString otpValue, CancellationToken cancellationToken) =>
        from otp in otpRepository
            .GetByValue(otpValue, cancellationToken)
            .ReduceTransformer(Error.New("The one-time password does not exist"))
        from user in userRepository
            .GetById(otp.UserId, cancellationToken)
            .ReduceTransformer(DomainErrors.UserNotFoundError)
        from utcNow in timeProvider.UtcNow
        from tokens in otp.IsExpired(utcNow)
            ? IO<UserSession>.Fail(Error.New("The one-time password has expired"))
            : userService.AddNewSessionToUser(user, cancellationToken)
        select tokens.AuthTokens;
}