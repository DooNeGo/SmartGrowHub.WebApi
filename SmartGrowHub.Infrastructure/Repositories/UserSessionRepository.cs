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

internal sealed class UserSessionRepository(ApplicationContext context) : IUserSessionRepository
{
    public IO<Unit> Add(UserSession session, CancellationToken cancellationToken) =>
        Add(session) >> SaveChanges(cancellationToken);

    public OptionT<IO, UserSession> GetById(Id<UserSession> id, CancellationToken cancellationToken) =>
        GetByPredicate(session => session.Id == id, cancellationToken);

    public OptionT<IO, UserSession> GetByRefreshTokenValue(Ulid value, CancellationToken cancellationToken) =>
        GetByPredicate(session => session.RefreshToken == value, cancellationToken);

    public IO<ImmutableArray<UserSession>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.UserSessions
            .Where(session => session.UserId == id)
            .ToListAsync(cancellationToken))
            .Bind(list => list
                .AsIterable()
                .Map(session => session.TryToDomain())
                .Traverse(Prelude.identity)
                .Map(iterable => iterable.ToImmutableArray())
                .As().ToIO());

    public IO<Unit> Remove(Id<UserSession> id, CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.UserSessions
            .Where(session => session.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public IO<Unit> Remove(UserSession session, CancellationToken cancellationToken) =>
        Remove(session) >> SaveChanges(cancellationToken);

    public IO<Unit> Update(UserSession session, CancellationToken cancellationToken) =>
        Update(session) >> SaveChanges(cancellationToken);

    private IO<Unit> Add(UserSession session) =>
        IO.lift(() => context.UserSessions.Add(session.ToDb()))
            .ToUnit();

    private IO<Unit> Remove(UserSession session) =>
        IO.lift(() => context.UserSessions.Remove(session.ToDb()))
            .ToUnit();

    private IO<Unit> Update(UserSession session) =>
        IO.lift(() => context.UserSessions.Update(session.ToDb()))
            .ToUnit();

    private IO<Unit> SaveChanges(CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.SaveChangesAsync(cancellationToken))
            .ToUnit();

    private OptionT<IO, UserSession> GetByPredicate(Expression<Func<UserSessionDb, bool>> predicate,
        CancellationToken cancellationToken) =>
        from value in OptionT<IO, UserSessionDb>.LiftIO(
            IO.liftAsync(() => context.UserSessions
                .FirstOrDefaultAsync(predicate, cancellationToken)
                .Map(Prelude.Optional)))
        from domainValue in value.TryToDomain().ToIO()
        select domainValue;
}
