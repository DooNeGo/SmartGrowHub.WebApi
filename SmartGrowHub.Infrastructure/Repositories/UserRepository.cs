using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Infrastructure.Data;
using System.Linq.Expressions;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public IO<Unit> Add(User user, CancellationToken cancellationToken) =>
        Add(user) >> SaveChanges(cancellationToken);

    public OptionT<IO, User> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.PhoneNumber == phoneNumber, cancellationToken);

    public OptionT<IO, User> GetByEmailAddress(EmailAddress email, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.EmailAddress == email, cancellationToken);

    public OptionT<IO, User> GetById(Id<User> id, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.Id == id, cancellationToken);

    public IO<Unit> Remove(Id<User> id, CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.Users
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public IO<Unit> Update(User user, CancellationToken cancellationToken) =>
        Update(user) >> SaveChanges(cancellationToken);

    private IO<Unit> Update(User user) =>
        IO.lift(() => context.Users.Update(user.ToDb())).ToUnit();

    private IO<Unit> SaveChanges(CancellationToken cancellationToken) =>
        IO.liftAsync(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit());

    private IO<Unit> Add(User user) =>
        IO.lift(() => context.Users.Add(user.ToDb())).ToUnit();

    private OptionT<IO, User> GetByPredicate(Expression<Func<UserDb, bool>> predicate,
        CancellationToken cancellationToken) =>
        from value in OptionT<IO, UserDb>.LiftIO(
            IO.liftAsync(() => context.Users
                .FirstOrDefaultAsync(predicate, cancellationToken)
                .Map(Prelude.Optional)))
        from domainValue in value.TryToDomain().ToIO()
        select domainValue;
}
