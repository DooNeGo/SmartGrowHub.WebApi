using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class UserSessionService(
    IUserSessionRepository sessionRepository,
    IUserService userService,
    ITokenService tokenService)
    : IUserSessionService
{
    public Eff<UserSession> CreateAsync(User user, CancellationToken cancellationToken)
        => sessionRepository.RemoveAllAsync(user.Id, cancellationToken)
            .Map(_ => tokenService.CreateTokens(user))
            .Map(tokens => UserSession.New(user.Id, tokens))
            .Bind(session => sessionRepository
                .Add(session)
                .Bind(_ => sessionRepository.SaveChangesAsync(cancellationToken))
                .Map(_ => session));

    public Eff<AuthTokens> RefreshTokensAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        => sessionRepository.GetAsync(refreshToken, cancellationToken)
            .Bind(session => userService.GetAsync(session.UserId, cancellationToken)
                .Map(user => tokenService.CreateTokens(user))
                .Bind(tokens => sessionRepository
                    .Update(session with { AuthTokens = tokens })
                    .Map(_ => tokens))
                .Bind(tokens => sessionRepository
                    .SaveChangesAsync(cancellationToken)
                    .Map(_ => tokens)));

    public Eff<Unit> RemoveAsync(Id<UserSession> id, CancellationToken cancellationToken) =>
        sessionRepository.RemoveAsync(id, cancellationToken);
}