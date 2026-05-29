namespace SmartGrowHub.Domain.Extensions;

public static class UlidExtensions
{
    extension(Ulid)
    {
        public static Fin<Ulid> From(string ulid) =>
            Ulid.TryParse(ulid, out Ulid result) ? result : Error.New("Invalid ulid");
    }
}