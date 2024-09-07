namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record UserDb(
    Ulid Id,
    string UserName,
    string Password,
    string Email,
    string DisplayName,
    IEnumerable<GrowHubDb> GrowHubs)
{
    private UserDb() : this(
        default!, default!,
        default!, default!,
        default!, [])
    { }    // Used by EF Core
}