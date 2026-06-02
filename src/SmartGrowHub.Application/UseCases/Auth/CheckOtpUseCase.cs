using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class CheckOtpUseCase(
    IOtpRepository otpRepository,
    ITokensIssuer tokensIssuer,
    IUserSessionRepository sessionRepository,
    IUserRepository userRepository,
    ITimeProvider timeProvider)
{
    public IO<AuthTokens> CheckOtp(NonEmptyString otpValue) =>
        from otp in otpRepository
            .GetByValue(otpValue)
            .ToIOOrFail(Error.New("The one-time password does not exist"))
        from user in userRepository
            .GetById(otp.UserId)
            .ToIOOrFail(DomainErrors.UserNotFoundError)
        from utcNow in timeProvider.UtcNow
        from tokens in otp.IsExpired(utcNow)
            ? IO.fail<UserSession>(Error.New("The one-time password has expired"))
            : AddNewSessionToUser(user)
        select tokens.AuthTokens;
    
    public IO<UserSession> AddNewSessionToUser(User user) =>
        from tokens in tokensIssuer.CreateTokens(user)
        let session = UserSession.New(user.Id, tokens)
        from _ in sessionRepository.AddAndSave(session)
        select session;
}