namespace SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

public sealed record WeeklyProgramDto(Ulid Id, IEnumerable<TimedQuantityDto<WeekTimeOnlyDto>> Entries)
    : IntervalProgramDto<WeekTimeOnlyDto>(Id, Entries);