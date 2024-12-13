using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class CheckOtpUseCase(
    IOtpRepository otpRepository,
    IUserRepository userRepository,
    IUserService userService)
{
    public Eff<AuthTokens> CheckOtp(int otpValue, CancellationToken cancellationToken) =>
        from otp in otpRepository.GetByValue(otpValue, cancellationToken)
        from user in userRepository.GetById(otp.UserId, cancellationToken)
        from tokens in userService.AddNewSessionToUser(user, cancellationToken)
        select tokens.AuthTokens;
}