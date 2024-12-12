using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using System.Linq.Expressions;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public Eff<Unit> Add(User user, CancellationToken cancellationToken) =>
        Add(user) >> SaveChanges(cancellationToken);

    public Eff<User> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.PhoneNumber == phoneNumber, cancellationToken);

    public Eff<User> GetByEmailAddress(EmailAddress email, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.EmailAddress == email, cancellationToken);

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
        liftEff(() => context.Users.Update(user.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> SaveChanges(CancellationToken cancellationToken) =>
        liftEff(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit());

    private Eff<Unit> Add(User user) =>
        liftEff(() => context.Users.Add(user.ToDb()))
            .Map(_ => unit);

    private Eff<User> GetByPredicate(Expression<Func<UserDb, bool>> predicate, CancellationToken cancellationToken) =>
        liftEff(() => context.Users
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .Map(Optional))
        .Bind(option => option.Match(
            Some: user => user.TryToDomain().ToEff(),
            None: DomainErrors.UserNotFoundError));
}
