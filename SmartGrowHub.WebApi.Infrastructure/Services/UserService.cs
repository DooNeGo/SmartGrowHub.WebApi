using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

public sealed class UserService(
    IUserRepository userRepository,
    IUserSessionRepository sessionRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    ITimeProvider timeProvider,
    TokenExpirationService tokenExpirationService)
    : IUserService
{
    public Eff<Unit> AddNewUser(User user, CancellationToken cancellationToken) =>
        from hashedPassword in passwordHasher.Hash(user.Password).ToEff()
        from _ in userRepository.Add(user.UpdatePassword(hashedPassword), cancellationToken)
        select unit;

    public Eff<UserSession> AddNewSessionToUser(User user, CancellationToken cancellationToken) =>
        from tokens in tokenService.CreateTokens(user)
        let session = UserSession.New(user.Id, tokens)
        from _ in sessionRepository.Add(session, cancellationToken)
        select session;

    public Eff<AuthTokens> RefreshTokens(RefreshToken oldToken, CancellationToken cancellationToken) =>
        from session in sessionRepository.GetByRefreshToken(oldToken, cancellationToken)
        from user in userRepository.GetById(session.UserId, cancellationToken)
        from newTokens in tokenService.CreateTokens(user)
        from utcNow in timeProvider.GetUtcNow()
        let currentRefreshToken = session.AuthTokens.RefreshToken
        from _1 in tokenExpirationService.ValidateRefreshToken(currentRefreshToken, utcNow).ToEff()
            | @catch(error => sessionRepository
                .Remove(session.Id, cancellationToken)
                .Bind(_ => FailEff<Unit>(error)))
        let updatedSession = session.UpdateTokens(newTokens)
        from _2 in sessionRepository.Update(updatedSession, cancellationToken)
        select newTokens;
}