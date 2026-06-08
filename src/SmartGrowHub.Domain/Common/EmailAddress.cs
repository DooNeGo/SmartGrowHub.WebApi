using System.Text.RegularExpressions;

namespace SmartGrowHub.Domain.Common;

public sealed partial record EmailAddress : DomainType<EmailAddress, string>
{
    private const string ErrorMessage = "Invalid email address format";
    
    [GeneratedRegex(@"^[a-zA-Z0-9_]+([-+.']\w+)*@[a-zA-Z0-9_]+([-.]\w+)*\.[a-zA-Z0-9_]+([-.]\w+)*$")]
    private static partial Regex EmailRegex { get; }

    private readonly string _value;

    private EmailAddress(string value) => _value = value;

    public static implicit operator string(EmailAddress email) => email.To();
    public static explicit operator EmailAddress(string value) => From(value).ThrowIfFail();

    public static Fin<EmailAddress> From(string rawValue) =>
        EmailRegex.IsMatch(rawValue)
            ? new EmailAddress(rawValue)
            : Fin.Fail<EmailAddress>(Error.New(ErrorMessage));

    public string To() => _value;

    public override string ToString() => _value;
}