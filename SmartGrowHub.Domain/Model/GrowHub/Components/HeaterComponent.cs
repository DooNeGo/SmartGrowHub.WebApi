using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class HeaterComponent(
    Id<HeaterComponent> id,
    ISetting setting)
    : Entity<HeaterComponent>(id), IGrowHubComponent
{
    public ISetting Setting { get; } = setting;
}
