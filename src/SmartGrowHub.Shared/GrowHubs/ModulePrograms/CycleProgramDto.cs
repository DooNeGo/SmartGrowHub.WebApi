namespace SmartGrowHub.Shared.GrowHubs.ModulePrograms;

public sealed record CycleProgramDto(Ulid Id, TimedQuantityDto<TimeOnly> CycleParameters) : ModuleProgramDto(Id);