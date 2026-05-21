namespace SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

public sealed record CycleProgramDto(Ulid Id, TimedQuantityDto<TimeOnly> CycleParameters) : ComponentProgramDto(Id);