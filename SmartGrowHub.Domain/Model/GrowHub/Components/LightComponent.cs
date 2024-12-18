using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class LightComponent(
    Id<LightComponent> id,
    ISetting setting,
    LightTypes lightType)
    : Entity<LightComponent>(id), IGrowHubComponent
{
    public ISetting Setting { get; } = setting;

    public LightTypes LightType { get; } = lightType;
}
