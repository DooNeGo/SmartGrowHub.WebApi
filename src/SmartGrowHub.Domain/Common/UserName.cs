namespace SmartGrowHub.Domain.Common;

public sealed record UserName : DomainType<UserName, string>
{
    private const string ErrorMessage = "Invalid username";

    private static readonly NonNegativeInteger MinimumLength = NonNegativeInteger.From(6).ThrowIfFail();
    
    private readonly string _value;

    private UserName(string value) => _value = value;

    public static implicit operator string(UserName userName) => userName.To();
    public static explicit operator UserName(string value) => From(value).ThrowIfFail();

    public static Fin<UserName> From(string rawValue)
    {
        Fin<UserName> result =
            from _1 in NonEmptyString.From(rawValue)
            from _2 in LatinString.From(rawValue)
            from _3 in BoundedString.From(rawValue, MinimumLength, None)
            select new UserName(rawValue);

        return result.MapFail(error => Error.New(ErrorMessage, error));
    }

    public string To() => _value;

    public override string ToString() => To();
}