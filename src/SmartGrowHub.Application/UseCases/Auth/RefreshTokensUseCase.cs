using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class RefreshTokensUseCase(
    IUserSessionRepository sessionRepository,
    IUserRepository userRepository,
    ITokensIssuer tokensIssuer,
    ITimeProvider timeProvider)
{
    public IO<AuthTokens> RefreshTokens(NonEmptyString oldToken) =>
        from session in sessionRepository
            .GetByRefreshTokenValue(oldToken)
            .ToIOOrFail(DomainErrors.SessionNotFoundError)
        from user in userRepository
            .GetById(session.UserId)
            .ToIOOrFail(DomainErrors.UserNotFoundError)
        from newTokens in tokensIssuer.CreateTokens(user)
        from utcNow in timeProvider.UtcNow
        from updatedSession in session.UpdateTokens(newTokens, utcNow).ToIO()
            .TapOnFail(_ => sessionRepository.RemoveByIdAndSave(session.Id))
        from _ in sessionRepository.UpdateAndSave(updatedSession)
        select newTokens;
}