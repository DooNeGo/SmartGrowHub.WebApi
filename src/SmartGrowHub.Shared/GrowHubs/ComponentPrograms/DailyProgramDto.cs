namespace SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

public sealed record DailyProgramDto(Ulid Id, IEnumerable<TimedQuantityDto<TimeOnly>> Entries)
    : IntervalProgramDto<TimeOnly>(Id, Entries);