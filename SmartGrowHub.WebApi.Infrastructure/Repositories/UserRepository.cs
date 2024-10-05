using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public Eff<Unit> Add(User user) =>
        user.TryToDb().ToEff()
            .Map(userDb => context.Add(userDb))
            .Map(_ => unit);

    public Eff<User> GetAsync(UserName userName, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .Where(user => user.UserName == userName.Value)
            .FirstOrDefaultAsync(cancellationToken)
            .Map(Optional))
            .MapFail(error => Error.New(new InternalException(error.ToException())))
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff(),
                None: Error.New(new ItemNotFoundException(nameof(User), None))));
     
    public Eff<User> GetAsync(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FindAsync([id.Value], cancellationToken).AsTask()
            .Map(Optional))
            .MapFail(error => Error.New(new InternalException(error.ToException())))
            .Bind(option => option.Match(
                Some: user => user.TryToDomain().ToEff(),
                None: Error.New(new ItemNotFoundException(nameof(User), None))));

    public Eff<Unit> SaveChangesAsync(CancellationToken cancellationToken) =>
        liftEff(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit());
}