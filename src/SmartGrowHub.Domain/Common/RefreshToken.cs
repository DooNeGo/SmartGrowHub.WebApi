namespace SmartGrowHub.Domain.Common;

public sealed record RefreshToken(Ulid Value, DateTime Expires)
    : DomainType<RefreshToken, (Ulid, DateTime)>
{
    public static Fin<RefreshToken> From((Ulid, DateTime) repr) => new RefreshToken(repr.Item1, repr.Item2);

    public (Ulid, DateTime) To() => (Value, Expires);

    public static RefreshToken New(DateTime expires) => new(Ulid.NewUlid(), expires);
}