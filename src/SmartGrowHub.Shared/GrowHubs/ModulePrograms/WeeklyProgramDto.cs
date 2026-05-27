namespace SmartGrowHub.Shared.GrowHubs.ModulePrograms;

public sealed record WeeklyProgramDto(Ulid Id, IEnumerable<TimedQuantityDto<WeekTimeOnlyDto>> Entries)
    : IntervalProgramDto<WeekTimeOnlyDto>(Id, Entries);