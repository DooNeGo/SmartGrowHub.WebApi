namespace SmartGrowHub.Domain.Common;

public sealed record NonEmptyString : DomainType<NonEmptyString, string>
{
    private const string ErrorMessage = "The value must not be empty or contain only spaces";

    private static readonly Error Error = Error.New(ErrorMessage);

    private readonly string _value;

    private NonEmptyString(string value) => _value = value;

    public static implicit operator string(NonEmptyString value) => value.To();
    public static explicit operator NonEmptyString(string value) => From(value).ThrowIfFail();

    public static Fin<NonEmptyString> From(string repr) =>
        !string.IsNullOrWhiteSpace(repr) ? new NonEmptyString(repr)
            : FinFail<NonEmptyString>(Error);

    public string To() => _value;

    public override string ToString() => To();
}