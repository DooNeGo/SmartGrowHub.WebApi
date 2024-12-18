using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class FanComponent(
    Id<FanComponent> id,
    ISetting setting)
    : Entity<FanComponent>(id), IGrowHubComponent
{
    public ISetting Setting { get; } = setting;
}
