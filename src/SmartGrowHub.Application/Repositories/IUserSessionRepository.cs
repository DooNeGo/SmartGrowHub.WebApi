using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.Application.Repositories;

public interface IUserSessionRepository
{
    IO<Unit> Add(UserSession userSession, CancellationToken cancellationToken);
    IO<Unit> Remove(Id<UserSession> id, CancellationToken cancellationToken);
    IO<Unit> Remove(UserSession session, CancellationToken cancellationToken);
    OptionT<IO, UserSession> GetByRefreshTokenValue(Ulid value, CancellationToken cancellationToken);
    OptionT<IO, UserSession> GetById(Id<UserSession> id, CancellationToken cancellationToken);
    IO<ImmutableArray<UserSession>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
    IO<Unit> Update(UserSession session, CancellationToken cancellationToken);
}
