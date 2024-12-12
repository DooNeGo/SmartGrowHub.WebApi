using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using System.Collections.Immutable;
using System.Linq.Expressions;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class UserSessionRepository(ApplicationContext context) : IUserSessionRepository
{
    public Eff<Unit> Add(UserSession session, CancellationToken cancellationToken) =>
        Add(session) >> SaveChanges(cancellationToken);

    public Eff<UserSession> GetById(Id<UserSession> id, CancellationToken cancellationToken) =>
        GetByPredicate(session => session.Id == id, cancellationToken);

    public Eff<UserSession> GetByRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken) =>
        GetByPredicate(session => session.RefreshToken == refreshToken.Ulid, cancellationToken);

    public Eff<ImmutableArray<UserSession>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.UserSessions
            .Where(session => session.UserDbId == id)
            .ToListAsync(cancellationToken))
            .Bind(list => list
                .Select(session => session.TryToDomain())
                .AsIterable()
                .Traverse(fin => fin)
                .Map(iterable => iterable.ToImmutableArray())
                .As().ToEff());

    public Eff<Unit> Remove(Id<UserSession> id, CancellationToken cancellationToken) =>
        liftEff(() => context.UserSessions
            .Where(session => session.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public Eff<Unit> Remove(UserSession session, CancellationToken cancellationToken) =>
        Remove(session) >> SaveChanges(cancellationToken);

    public Eff<Unit> Update(UserSession session, CancellationToken cancellationToken) =>
        Update(session) >> SaveChanges(cancellationToken);

    private Eff<Unit> Add(UserSession session) =>
        liftEff(() => context.UserSessions.Add(session.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> Remove(UserSession session) =>
        liftEff(() => context.UserSessions.Remove(session.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> Update(UserSession session) =>
        liftEff(() => context.UserSessions.Update(session.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> SaveChanges(CancellationToken cancellationToken) =>
        liftEff(() => context.SaveChangesAsync(cancellationToken))
            .Map(_ => unit);

    private Eff<UserSession> GetByPredicate(Expression<Func<UserSessionDb, bool>> predicate, CancellationToken cancellationToken) =>
        liftEff(() => context.UserSessions
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .Map(Optional))
        .Bind(option => option.Match(
            Some: session => session.TryToDomain().ToEff(),
            None: DomainErrors.SessionNotFoundError));
}
