using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IUserRepository : IRepository<User>
{
    OptionT<IO, User> GetByPhoneNumber(PhoneNumber phoneNumber);
    OptionT<IO, User> GetByEmailAddress(EmailAddress email);
}