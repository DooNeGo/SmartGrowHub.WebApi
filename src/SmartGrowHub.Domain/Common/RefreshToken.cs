using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Domain.Common;

public sealed record RefreshToken(NonEmptyString Value, DateTime Expires)
    : DomainType<RefreshToken, (string, DateTime)>
{
    public static Fin<RefreshToken> From((string, DateTime) repr) => (
        from nonEmpty in NonEmptyString.From(repr.Item1)
        from _ in Ulid.From(repr.Item1)
        select new RefreshToken(nonEmpty, repr.Item2)
    ).MapFail(error => Error.New("Invalid refresh token", error));

    public (string, DateTime) To() => (Value, Expires);

    public static RefreshToken New(DateTime expires) =>
        new(NonEmptyString.From(Ulid.NewUlid().ToString()).ThrowIfFail(), expires);
}