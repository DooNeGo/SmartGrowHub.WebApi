using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;

namespace SmartGrowHub.Domain.Model;

public sealed class User : Entity<User>
{
    private User(Id<User> id,
        Option<EmailAddress> emailAddress,
        Option<PhoneNumber> phoneNumber) : base(id)
    {
        Email = emailAddress;
        PhoneNumber = phoneNumber;
    }
    
    private User(User original) : this(
        original.Id, original.Email,
        original.PhoneNumber)
    { }

    public Option<EmailAddress> Email { get; init; }
    
    public Option<PhoneNumber> PhoneNumber { get; init; }

    public static Fin<User> Create(Id<User> id, Option<EmailAddress> emailAddress, Option<PhoneNumber> phoneNumber) =>
        emailAddress.IsNone && phoneNumber.IsNone
            ? new UnexpectedError("The user must have phone number or email")
            : new User(id, emailAddress, phoneNumber);

    public static User NewFromEmailAddress(EmailAddress email) => New(email, None);
    
    public static User NewFromPhoneNumber(PhoneNumber phoneNumber) => New(None, phoneNumber);
    
    private static User New(Option<EmailAddress> email, Option<PhoneNumber> phoneNumber) =>
        new(new Id<User>(Ulid.NewUlid()), email, phoneNumber);
}