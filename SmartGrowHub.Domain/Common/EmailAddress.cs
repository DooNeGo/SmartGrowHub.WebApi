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
        Fin<EmailAddress> result =
            from nonEmpty in NonEmptyString.From(rawValue.Trim())
            from _ in IsValidEmailAddress(nonEmpty)
            from latin in LatinString.From(nonEmpty)
            select new EmailAddress(latin);

        return result.MapFail(error => Error.New(ErrorMessage, error));
    }

    private static Fin<string> IsValidEmailAddress(string value) =>
        Attribute.IsValid(value) ? value : FinFail<string>(Error.New(ErrorMessage));

    public string To() => _value;

    public override string ToString() => _value;
}