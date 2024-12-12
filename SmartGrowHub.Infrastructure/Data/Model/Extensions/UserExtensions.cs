using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class UserExtensions
{
    public static UserDb ToDb(this User user) => new()
    {
        Id = user.Id,
        EmailAddress = user.Email
            .Map(email => email.To())
            .IfNone(string.Empty),
        PhoneNumber = user.PhoneNumber
            .Map(phoneNumber => phoneNumber.To())
            .IfNone(string.Empty)
    };

    public static Fin<User> TryToDomain(this UserDb user)
    {
        var id = new Id<User>(user.Id);
        
        Option<EmailAddress> email = EmailAddress.From(user.EmailAddress).ToOption();
        Option<PhoneNumber> phoneNumber = PhoneNumber.From(user.PhoneNumber).ToOption();
        
        return User.Create(id, email, phoneNumber);
    }
}