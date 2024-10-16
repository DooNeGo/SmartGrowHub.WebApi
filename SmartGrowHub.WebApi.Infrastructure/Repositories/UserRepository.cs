using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.Users.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.Model;
using SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;
using System.Linq.Expressions;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public Eff<Unit> Add(User user, CancellationToken cancellationToken) =>
        Add(user) >> SaveChanges(cancellationToken);

    public Eff<User> GetByUserName(UserName userName, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.UserName == userName, cancellationToken);

    public Eff<User> GetById(Id<User> id, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.Id == id, cancellationToken);

    public Eff<Unit> Remove(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public Eff<Unit> Update(User user, CancellationToken cancellationToken) =>
        Update(user) >> SaveChanges(cancellationToken);

    private Eff<Unit> Update(User user) =>
        user.TryToDb().ToEff()
            .Map(context.Users.Update)
            .Map(_ => unit);

    private Eff<Unit> SaveChanges(CancellationToken cancellationToken) =>
        liftEff(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit());

    private Eff<Unit> Add(User user) =>
        user.TryToDb().ToEff()
            .Map(context.Users.Add)
            .Map(_ => unit);

    private Eff<User> GetByPredicate(Expression<Func<UserDb, bool>> predicate, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .Map(Optional))
        .Bind(option => option.Match(
            Some: user => user.TryToDomain().ToEff(),
            None: DomainErrors.UserNotFoundError));
}
