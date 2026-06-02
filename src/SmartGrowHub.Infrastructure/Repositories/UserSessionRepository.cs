using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using System.Collections.Immutable;
using System.Linq.Expressions;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class UserSessionRepository : Repository<UserSession, UserSessionDb>, IUserSessionRepository
{
    private readonly ApplicationContext _context;

    public UserSessionRepository(ApplicationContext context) : base(context) => _context = context;

    public OptionT<IO, UserSession> GetByRefreshTokenValue(NonEmptyString value) =>
        GetByPredicate(session => session.RefreshToken == value);

    public IO<ImmutableList<UserSession>> GetAllByUserId(Id<User> id) =>
        IO.liftAsync(env => _context.UserSessions
                .Where(session => session.UserId == id)
                .ToListAsync(env.Token))
            .Bind(list => list
                .AsIterable()
                .Traverse(session => session.TryToDomain())
                .Map(iterable => iterable.ToImmutableList())
                .As().ToIO());

    protected override UserSessionDb ToDb(UserSession domain) => domain.ToDb();

    protected override Fin<UserSession> ToDomain(UserSessionDb db) => db.TryToDomain();

    protected override IQueryable<UserSessionDb> AddIncludes(IQueryable<UserSessionDb> query) => query;
}
