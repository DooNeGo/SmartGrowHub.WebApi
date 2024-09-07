using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record ComponentDb(
    Ulid Id,
    ComponentType Type,
    int Value,
    string Unit,
    Ulid SettingId,
    SettingDb Setting)
{
    private ComponentDb() : this(
        default!, default,
        default, default!,
        default, default!)
    { } // Used by EF Core
}
