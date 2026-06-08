using System.Text.RegularExpressions;

namespace SmartGrowHub.Domain.Common;

public sealed partial record LatinString : DomainType<LatinString, string>
{
    private const string ErrorMessage =
        "The value must consist only of Latin letters, digits and special characters";

    [GeneratedRegex(@"^[a-zA-Z0-9!@#$%^&*()\-_=+\[\]{}|\\:;""'<>,.?/~]*$")]
    private static partial Regex LatinRegex { get; }
    
    private readonly string _value;

    private LatinString(string value) => _value = value;

    public static implicit operator string(LatinString value) => value.To();
    public static explicit operator LatinString(string value) => From(value).ThrowIfFail();

    public static Fin<LatinString> From(string rawValue) =>
        LatinRegex.IsMatch(rawValue)
            ? Fin.Succ(new LatinString(rawValue))
            : Fin.Fail<LatinString>(Error.New(ErrorMessage));

    public string To() => _value;

    public override string ToString() => To();
}