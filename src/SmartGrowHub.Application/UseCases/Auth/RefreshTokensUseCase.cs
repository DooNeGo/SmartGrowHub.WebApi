using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class RefreshTokensUseCase
{
    private readonly IUserSessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokensIssuer _tokensIssuer;
    private readonly ITimeProvider _timeProvider;

    public RefreshTokensUseCase(IUserSessionRepository sessionRepository,
        IUserRepository userRepository,
        ITokensIssuer tokensIssuer,
        ITimeProvider timeProvider)
    {
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _tokensIssuer = tokensIssuer;
        _timeProvider = timeProvider;
    }

    public IO<AuthTokens> RefreshTokens(Ulid oldToken) =>
        from session in _sessionRepository
            .GetByRefreshTokenValue(oldToken)
            .ToIOOrFail(DomainErrors.SessionNotFoundError)
        from user in _userRepository
            .GetById(session.UserId)
            .ToIOOrFail(DomainErrors.UserNotFoundError)
        from newTokens in _tokensIssuer.CreateTokens(user)
        from utcNow in _timeProvider.UtcNow
        from updatedSession in session.UpdateTokens(newTokens, utcNow).ToIO()
            .TapOnFail(_ => _sessionRepository.RemoveByIdAndSave(session.Id))
        from _ in _sessionRepository.UpdateAndSave(updatedSession)
        select newTokens;
}