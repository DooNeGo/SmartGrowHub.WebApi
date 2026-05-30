using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IUserRepository : IRepository<User>
{
    OptionT<IO, User> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    OptionT<IO, User> GetByEmailAddress(EmailAddress email, CancellationToken cancellationToken);
    OptionT<IO, User> GetById(Id<User> id, CancellationToken cancellationToken);
}