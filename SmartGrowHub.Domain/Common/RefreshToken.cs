namespace SmartGrowHub.Domain.Common;

public sealed record RefreshToken(Ulid Ulid, DateTime Expires)
    : DomainType<RefreshToken, (string, DateTime)>
{
    public static Fin<RefreshToken> From((string, DateTime) repr) =>
        Ulid.TryParse(repr.Item1, out Ulid ulid)
            ? new RefreshToken(ulid, repr.Item2)
            : Error.New("Invalid refresh token");

    public (string, DateTime) To() => (Ulid.ToString(), Expires);
    
    public static RefreshToken New(DateTime expires) => new(Ulid.NewUlid(), expires);
}