using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.Application.Repositories;

public interface IUserSessionRepository : IRepository<UserSession>
{
    OptionT<IO, UserSession> GetByRefreshTokenValue(NonEmptyString value, CancellationToken cancellationToken);
    OptionT<IO, UserSession> GetById(Id<UserSession> id, CancellationToken cancellationToken);
    IO<ImmutableArray<UserSession>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
}
