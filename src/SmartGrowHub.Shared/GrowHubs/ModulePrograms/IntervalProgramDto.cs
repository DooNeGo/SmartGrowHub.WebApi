namespace SmartGrowHub.Shared.GrowHubs.ModulePrograms;

public abstract record IntervalProgramDto<TTime>(Ulid Id, IEnumerable<TimedQuantityDto<TTime>> Entries)
    : ModuleProgramDto(Id);