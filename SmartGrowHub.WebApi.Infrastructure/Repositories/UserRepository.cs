using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.Model;
using SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;
using SmartGrowHub.WebApi.Infrastructure.Repositories.Extensions;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public Eff<Unit> Add(User user, CancellationToken cancellationToken) =>
        Add(user) >> SaveChanges(cancellationToken)
            .MapFail(error => Id(error.ToException())
                .Map<Exception>(exception => exception is UniqueConstraintException
                    ? new UserAlreadyExistException()
                    : new InternalException(exception))
                .Value);

    public Eff<User> GetByUserName(UserName userName, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FirstOrDefaultAsync(user => user.UserName == userName, cancellationToken)
            .Map(Optional))
            .MapFail(error => Error.New(new InternalException(error.ToException())))
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff(),
                None: Error.New(new ItemNotFoundException(nameof(User), None))));

    public Eff<User> GetById(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FindAsync([id.Value], cancellationToken).AsTask()
            .Map(Optional))
            .MapFail(error => Error.New(new InternalException(error.ToException())))
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff(),
                None: Error.New(new ItemNotFoundException(nameof(User), None))));

    public Eff<User> GetBySessionId(Id<UserSession> id, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FirstOrDefaultAsync(user => user.Sessions
                .Any(session => session.Id == id), cancellationToken)
            .Map(Optional))
            .MapFail(error => Error.New(new InternalException(error.ToException())))
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff(),
                None: Error.New("Invalid session id")));

    public Eff<User> GetByRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FirstOrDefaultAsync(user => user.Sessions
                .Any(session => session.RefreshToken == refreshToken), cancellationToken)
            .Map(Optional))
            .MapFail(error => Error.New(new InternalException(error.ToException())))
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff(),
                None: Error.New("Invalid refresh token")));

    public Eff<Unit> Remove(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public Eff<Unit> Update(User user, CancellationToken cancellationToken) =>
        Update(user) >> SaveChanges(cancellationToken);

    private Eff<Unit> Add(User user) =>
        user.TryToDb().ToEff()
            .Map(context.Users.Add)
            .Map(_ => unit);

    private Eff<Unit> Update(User user) =>
        user.TryToDb()
            .Map(updatedUser => context.Users.Local
                .FindById(updatedUser.Id)
                .Match(
                    Some: savedUser => UpdateUserBasedOnAnotherUser(savedUser, updatedUser),
                    None: updatedUser))
            .Map(_ => unit);

    private Eff<Unit> SaveChanges(CancellationToken cancellationToken) =>
        liftEff(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit());

    private static UserDb UpdateUserBasedOnAnotherUser(UserDb forUpdate, UserDb basedOn)
    {
        forUpdate.UserName = basedOn.UserName;
        forUpdate.Password = basedOn.Password;
        forUpdate.EmailAddress = basedOn.EmailAddress;
        forUpdate.DisplayName = basedOn.DisplayName;
        forUpdate.Sessions = basedOn.Sessions;
        forUpdate.GrowHubs = basedOn.GrowHubs;

        return forUpdate;
    }
}
