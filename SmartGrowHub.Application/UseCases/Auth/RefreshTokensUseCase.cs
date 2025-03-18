using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class RefreshTokensUseCase(
    IUserSessionRepository sessionRepository,
    IUserRepository userRepository,
    ITokensIssuer tokensIssuer,
    ITimeProvider timeProvider)
{
    public IO<AuthTokens> RefreshTokens(Ulid oldToken, CancellationToken cancellationToken) =>
        from session in sessionRepository
            .GetByRefreshTokenValue(oldToken, cancellationToken)
            .ReduceTransformer(DomainErrors.SessionNotFoundError)
        from user in userRepository
            .GetById(session.UserId, cancellationToken)
            .ReduceTransformer(DomainErrors.UserNotFoundError)
        from newTokens in tokensIssuer.CreateTokens(user)
        from utcNow in timeProvider.UtcNow
        from updatedSession in session.UpdateTokens(newTokens, utcNow).ToIO()
            .TapOnFail(_ => sessionRepository.Remove(session.Id, cancellationToken))
        from _ in sessionRepository.Update(updatedSession, cancellationToken)
        select newTokens;
}