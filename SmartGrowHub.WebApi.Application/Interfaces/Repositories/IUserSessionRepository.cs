using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserSessionRepository
{
    Eff<Unit> Add(UserSession userSession, CancellationToken cancellationToken);
    Eff<Unit> Remove(Id<UserSession> id, CancellationToken cancellationToken);
    Eff<Unit> Remove(UserSession session, CancellationToken cancellationToken);
    Eff<UserSession> GetByRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    Eff<UserSession> GetById(Id<UserSession> id, CancellationToken cancellationToken);
    Eff<ImmutableArray<UserSession>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> Update(UserSession session, CancellationToken cancellationToken);
}
