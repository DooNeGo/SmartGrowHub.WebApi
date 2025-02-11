using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class DayLightComponent(
    Id<DayLightComponent> id,
    Setting setting)
    : Entity<DayLightComponent>(id)
{
    public Setting Setting { get; } = setting;
}