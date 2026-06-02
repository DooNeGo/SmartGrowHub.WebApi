using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.Application.Repositories;

public interface IUserSessionRepository : IRepository<UserSession>
{
    OptionT<IO, UserSession> GetByRefreshTokenValue(NonEmptyString value);
    IO<ImmutableList<UserSession>> GetAllByUserId(Id<User> id);
}
