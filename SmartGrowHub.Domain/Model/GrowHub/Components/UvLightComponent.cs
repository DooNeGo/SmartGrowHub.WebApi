using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class UvLightComponent(
    Id<UvLightComponent> id,
    Setting setting)
    : Entity<UvLightComponent>(id)
{
    public Setting Setting { get; } = setting;
}