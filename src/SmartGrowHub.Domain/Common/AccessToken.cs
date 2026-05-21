namespace SmartGrowHub.Domain.Common;

public sealed record AccessToken : DomainType<AccessToken, string>
{
    private AccessToken(string value) => Value = value;

    public string Value { get; }

    public static implicit operator string(AccessToken token) => token.Value;
    public static explicit operator AccessToken(string value) => From(value).ThrowIfFail();

    public static Fin<AccessToken> From(string repr) =>
        from nonEmptyString in NonEmptyString.From(repr)
        select new AccessToken(nonEmptyString);

    public string To() => Value;

    public override string ToString() => Value;
}