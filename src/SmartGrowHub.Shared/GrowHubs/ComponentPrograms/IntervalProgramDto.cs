namespace SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

public abstract record IntervalProgramDto<TTime>(Ulid Id, IEnumerable<TimedQuantityDto<TTime>> Entries)
    : ComponentProgramDto(Id);