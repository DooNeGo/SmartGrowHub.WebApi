using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

public sealed class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    ITimeProvider timeProvider)
    : IUserService
{
    public Eff<User> GetByUserName(UserName userName, CancellationToken cancellationToken) =>
        userRepository.GetByUserName(userName, cancellationToken);

    public Eff<Unit> AddNewUser(User user, CancellationToken cancellationToken) =>
        from hashedPassword in passwordHasher.Hash(user.Password).ToEff()
        from _ in userRepository.Add(user with { Password = hashedPassword }, cancellationToken)
        select unit;

    public Eff<User> RemoveSession(Id<UserSession> sessionId, CancellationToken cancellationToken) =>
        from user in userRepository.GetBySessionId(sessionId, cancellationToken)
        from updatedUser in user.RemoveSession(sessionId).ToEff()
        from _ in userRepository.Update(updatedUser, cancellationToken)
        select updatedUser;

    public Eff<(User, UserSession)> AddNewSessionToUser(User user, CancellationToken cancellationToken) =>
        from tokens in tokenService.CreateTokens(user)
        let session = UserSession.New(user.Id, tokens)
        from updatedUser in user.AddSession(session).ToEff()
        from _ in userRepository.Update(updatedUser, cancellationToken)
        select (updatedUser, session);

    public Eff<AuthTokens> RefreshTokens(RefreshToken oldToken, CancellationToken cancellationToken) =>
        from user in userRepository.GetByRefreshToken(oldToken, cancellationToken)
        from tokens in tokenService.CreateTokens(user)
        from utcNow in timeProvider.GetUtcNow()
        from result in user.UpdateSessionTokens(oldToken, tokens, utcNow).ToEff()
        from _ in userRepository.Update(result.Item1, cancellationToken) >>
            result.Item2.Match(
                Some: FailEff<Unit>,
                None: () => unitEff)
        select tokens;
}