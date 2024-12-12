using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.Domain.Common;

public sealed record PhoneNumber : DomainType<PhoneNumber, string>
{
    private const string ErrorMessage = "Invalid phone number";

    private static readonly PhoneAttribute PhoneAttribute = new();

    private readonly string _value;

    private PhoneNumber(string value) => _value = value;

    public static implicit operator string(PhoneNumber userName) => userName.To();
    public static explicit operator PhoneNumber(string value) => From(value).ThrowIfFail();

    public static Fin<PhoneNumber> From(string repr) =>
        from nonEmpty in NonEmptyString.From(repr).MapFail(error => Error.New(ErrorMessage, error))
        from phoneNumber in PhoneAttribute.IsValid(repr)
            ? FinSucc(new PhoneNumber(repr))
            : FinFail<PhoneNumber>(Error.New(ErrorMessage))
        select phoneNumber;

    public string To() => _value;
}
