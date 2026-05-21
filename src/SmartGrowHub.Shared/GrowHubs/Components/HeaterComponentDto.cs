using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Shared.GrowHubs.Components;

public sealed record HeaterComponentDto(Ulid Id, ComponentProgramDto Program) : GrowHubComponentDto(Id, Program);
