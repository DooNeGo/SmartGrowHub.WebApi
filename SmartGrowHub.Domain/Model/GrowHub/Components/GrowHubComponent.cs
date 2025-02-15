using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public abstract class GrowHubComponent(Id<GrowHubComponent> id, ComponentProgram program) : Entity<GrowHubComponent>(id)
{
    public ComponentProgram Program { get; } = program;
}
