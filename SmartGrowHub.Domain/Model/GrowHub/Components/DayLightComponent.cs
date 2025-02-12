using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class DayLightComponent(
    Id<DayLightComponent> id,
    ComponentProgram program)
    : Entity<DayLightComponent>(id)
{
    public ComponentProgram Program { get; } = program;
}