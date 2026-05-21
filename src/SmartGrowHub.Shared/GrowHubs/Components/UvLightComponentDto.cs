using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Shared.GrowHubs.Components;

public sealed record UvLightComponentDto(Ulid Id, ComponentProgramDto Program) : GrowHubComponentDto(Id, Program);