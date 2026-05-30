using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : Repository<User, UserDb>(context), IUserRepository
{
    public OptionT<IO, User> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.PhoneNumber == phoneNumber, cancellationToken);

    public OptionT<IO, User> GetByEmailAddress(EmailAddress email, CancellationToken cancellationToken) =>
        GetByPredicate(user => user.EmailAddress == email, cancellationToken);

    protected override UserDb ToDb(User domain) => domain.ToDb();

    protected override Fin<User> ToDomain(UserDb db) => db.TryToDomain();

    protected override IQueryable<UserDb> AddIncludes(IQueryable<UserDb> query) => query;
}
