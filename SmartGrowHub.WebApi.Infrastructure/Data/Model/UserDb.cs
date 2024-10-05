namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record UserDb(
    Ulid Id,
    string UserName,
    byte[] Password,
    string Email,
    string DisplayName)
{
    private UserDb() : this(
        default, default!,
        default!, default!,
        default!)
    { } // Used by EF Core

    public IEnumerable<GrowHubDb> GrowHubs { get; } = [];
}