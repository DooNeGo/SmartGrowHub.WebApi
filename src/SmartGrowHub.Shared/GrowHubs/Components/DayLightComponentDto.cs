using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Shared.GrowHubs.Components;

public sealed record DayLightComponentDto(Ulid Id, ComponentProgramDto Program) : GrowHubComponentDto(Id, Program);