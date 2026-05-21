using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Shared.GrowHubs.Components;

public sealed record FanComponentDto(Ulid Id, ComponentProgramDto Program) : GrowHubComponentDto(Id, Program);
