using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class RefreshTokensUseCase(
    IUserSessionRepository sessionRepository,
    IUserRepository userRepository,
    ITokensIssuer tokensIssuer,
    ITimeProvider timeProvider)
{
    public Eff<AuthTokens> RefreshTokens(Ulid oldToken, CancellationToken cancellationToken) =>
        from session in sessionRepository.GetByRefreshTokenValue(oldToken, cancellationToken)
        from user in userRepository.GetById(session.UserId, cancellationToken)
        from newTokens in tokensIssuer.CreateTokens(user)
        from utcNow in timeProvider.UtcNow
        from updatedSession in session.UpdateTokens(newTokens, utcNow).ToEff()
            | @catch(error => sessionRepository
                .Remove(session.Id, cancellationToken)
                .Bind(_ => FailEff<UserSession>(error)))
        from _2 in sessionRepository.Update(updatedSession, cancellationToken)
        select newTokens;
}