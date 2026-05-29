using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Shared.GrowHubs.Requests;

public sealed record SetModuleProgramRequestDto(
    ProgramTypeDto Type,
    QuantityDto? ManualEntry = null,
    IReadOnlyList<TimedQuantityDto<TimeOnly>>? DailyEntries = null,
    IReadOnlyList<TimedQuantityDto<WeekTimeOnlyDto>>? WeeklyEntries = null);