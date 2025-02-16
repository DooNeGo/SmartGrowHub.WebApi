using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class CheckOtpUseCase(
    IOtpRepository otpRepository,
    IUserRepository userRepository,
    IUserService userService,
    ITimeProvider timeProvider)
{
    public Eff<AuthTokens> CheckOtp(NonEmptyString otpValue, CancellationToken cancellationToken) =>
        from otp in otpRepository.GetByValue(otpValue, cancellationToken)
        from user in userRepository.GetById(otp.UserId, cancellationToken)
        from utcNow in timeProvider.UtcNow
        from tokens in otp.IsExpired(utcNow)
            ? FailEff<UserSession>(Error.New("The one-time password has expired"))
            : userService.AddNewSessionToUser(user, cancellationToken)
        select tokens.AuthTokens;
}