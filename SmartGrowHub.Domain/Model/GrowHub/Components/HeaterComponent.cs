using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class HeaterComponent(
    Id<HeaterComponent> id,
    ComponentProgram program)
    : Entity<HeaterComponent>(id)
{
    public ComponentProgram Program { get; } = program;
}
