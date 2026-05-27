namespace SmartGrowHub.Shared.GrowHubs.ModulePrograms;

public sealed record DailyProgramDto(Ulid Id, IEnumerable<TimedQuantityDto<TimeOnly>> Entries)
    : IntervalProgramDto<TimeOnly>(Id, Entries);