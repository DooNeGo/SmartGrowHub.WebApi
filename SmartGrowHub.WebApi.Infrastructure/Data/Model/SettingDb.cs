using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record SettingDb(
    Ulid Id,
    SettingType Type,
    SettingMode Mode,
    ImmutableArray<ComponentDb> Components,
    Ulid GrowHubId,
    GrowHubDb GrowHub)
{
    private SettingDb() : this(
        default!, default,
        default, [],
        default, default!)
    { }     // Used by EF Core
}
