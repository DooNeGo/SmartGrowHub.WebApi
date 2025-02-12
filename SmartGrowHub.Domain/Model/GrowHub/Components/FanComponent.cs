using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class FanComponent(
    Id<FanComponent> id,
    ComponentProgram program)
    : Entity<FanComponent>(id)
{
    public ComponentProgram Program { get; } = program;
}
