using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IUserRepository
{
    Eff<User> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Eff<User> GetByEmailAddress(EmailAddress email, CancellationToken cancellationToken);
    Eff<User> GetById(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> Add(User user, CancellationToken cancellationToken);
    Eff<Unit> Remove(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> Update(User user, CancellationToken cancellationToken);
}