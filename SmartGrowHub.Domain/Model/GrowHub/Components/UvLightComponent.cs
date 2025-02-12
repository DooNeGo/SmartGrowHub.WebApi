using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class UvLightComponent(
    Id<UvLightComponent> id,
    ComponentProgram program)
    : Entity<UvLightComponent>(id)
{
    public ComponentProgram Program { get; } = program;
}