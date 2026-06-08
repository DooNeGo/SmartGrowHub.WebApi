using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.Domain.Common;

public sealed record EmailAddress : DomainType<EmailAddress, string>
{
    private const string ErrorMessage = "Invalid email address format";

    private static readonly EmailAddressAttribute Attribute = new();

    private readonly string _value;

    private EmailAddress(string value) => _value = value;

    public static implicit operator string(EmailAddress email) => email.To();
    public static explicit operator EmailAddress(string value) => From(value).ThrowIfFail();

    public static Fin<EmailAddress> From(string rawValue)
    {
        rawValue = rawValue.Trim();
        
        Fin<EmailAddress> result =
            from _1 in NonEmptyString.From(rawValue)
            from _2 in ValidateEmailAddress(rawValue)
            from _3 in LatinString.From(rawValue)
            select new EmailAddress(rawValue);

        return result.MapFail(error => Error.New(ErrorMessage, error));
    }

    private static Fin<Unit> ValidateEmailAddress(string value) =>
        Attribute.IsValid(value) ? unit : Fin.Fail<Unit>(Error.New(ErrorMessage));

    public string To() => _value;

    public override string ToString() => _value;
}