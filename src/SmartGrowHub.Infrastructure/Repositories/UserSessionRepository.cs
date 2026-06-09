using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using System.Collections.Immutable;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class UserSessionRepository : Repository<UserSession, UserSessionDb>, IUserSessionRepository
{
    public UserSessionRepository(ApplicationContext context) : base(context) { }

    public OptionT<IO, UserSession> GetByRefreshTokenValue(Ulid value) =>
        GetByPredicate(session => session.RefreshToken == value);

    public IO<ImmutableList<UserSession>> GetAllByUserId(Id<User> id) =>
        GetAllByPredicate(session => session.UserId == id);

    protected override UserSessionDb ToDb(UserSession domain) => domain.ToDb();

    protected override Fin<UserSession> ToDomain(UserSessionDb db) => db.TryToDomain();

    protected override IQueryable<UserSessionDb> AddIncludes(IQueryable<UserSessionDb> query) => query;
}
