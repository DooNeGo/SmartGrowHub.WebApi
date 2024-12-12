using System.Text.RegularExpressions;

namespace SmartGrowHub.Domain.Common;

public sealed partial record LatinString : DomainType<LatinString, string>
{
    private const string ErrorMessage =
        "The value must consist only of Latin letters, digits and special characters";

    private readonly string _value;

    private LatinString(string value) => _value = value;

    public static implicit operator string(LatinString value) => value.To();
    public static explicit operator LatinString(string value) => From(value).ThrowIfFail();

    public static Fin<LatinString> From(string rawValue) =>
        GetLatinRegex().IsMatch(rawValue)
            ? FinSucc(new LatinString(rawValue))
            : FinFail<LatinString>(Error.New(ErrorMessage));

    public string To() => _value;

    public override string ToString() => To();

    [GeneratedRegex(@"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|\\:;""'<>,.?/~]*$", RegexOptions.Compiled)]
    private static partial Regex GetLatinRegex();
}