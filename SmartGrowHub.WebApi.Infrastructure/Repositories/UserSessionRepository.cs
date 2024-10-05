using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories;

internal sealed class UserSessionRepository(ApplicationContext context) : IUserSessionRepository
{
    public Eff<Unit> Add(UserSession userSession) =>
        liftEff(() => context.Add(userSession.ToDb()))
            .Map(_ => unit)
            .MapFail(error => Error.New(new InternalException(error.ToException())));

    public Eff<Unit> Update(UserSession userSession) =>
        liftEff(() => context.Update(userSession.ToDb()))
            .Map(_ => unit)
            .MapFail(error => Error.New(new InternalException(error.ToException())));

    public Eff<Unit> RemoveAsync(Id<UserSession> id, CancellationToken cancellationToken) =>
        liftEff(() => context.UserSessions
            .Where(session => session.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public Eff<UserSession> GetAsync(RefreshToken token, CancellationToken cancellationToken) =>
        liftEff(() => context.UserSessions
            .Where(session => session.RefreshToken == token.Ulid)
            .FirstOrDefaultAsync(cancellationToken)
            .Map(Optional))
        .MapFail(error => Error.New(new InternalException(error.ToException())))
        .Bind(option => option.Match(
            Some: session => session.TryToDomain().ToEff(),
            None: Error.New("Invalid refresh token")));

    public Eff<Unit> RemoveAllAsync(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.UserSessions
            .Where(session => session.UserId == id.Value)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit())
            .MapFail(error => Error.New(new InternalException(error.ToException())));

    public Eff<Unit> SaveChangesAsync(CancellationToken cancellationToken) =>
        liftEff(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit())
            .MapFail(error => Error.New(new InternalException(error.ToException())));
}
