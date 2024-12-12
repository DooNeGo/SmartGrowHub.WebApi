namespace SmartGrowHub.Domain.Common;

public sealed record UserName : DomainType<UserName, string>
{
    private const int MinimumLength = 6;
    private const string ErrorMessage = "Invalid username";

    private readonly string _value;

    private UserName(string value) => _value = value;

    public static implicit operator string(UserName userName) => userName.To();
    public static explicit operator UserName(string value) => From(value).ThrowIfFail();

    public static Fin<UserName> From(string rawValue)
    {
        Fin<UserName> result =
            from nonEmpty in NonEmptyString.From(rawValue.Trim())
            from latin in LatinString.From(nonEmpty)
            from minLength in NonNegativeInteger.From(MinimumLength)
            from bounded in BoundedString.From(latin, minLength, None)
            select new UserName(bounded);

        return result.MapFail(error => Error.New(ErrorMessage, error));
    }

    public string To() => _value;

    public override string ToString() => To();
}