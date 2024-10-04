namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record UserSessionDb(
    Ulid Id,
    Ulid UserId,
    string AccessToken,
    Ulid RefreshToken,
    DateTime Expires)
{
    private UserSessionDb() : this(
        default, default,
        default!, default,
        default)
    { } // Used by EF Core

    public UserDb User { get; } = null!;
}