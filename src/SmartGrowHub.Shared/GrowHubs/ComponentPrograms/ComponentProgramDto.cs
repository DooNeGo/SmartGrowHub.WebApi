using System.Text.Json.Serialization;

namespace SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

[JsonDerivedType(typeof(ManualProgramDto), "Manual")]
[JsonDerivedType(typeof(CycleProgramDto), "Cycle")]
[JsonDerivedType(typeof(DailyProgramDto), "Daily")]
[JsonDerivedType(typeof(WeeklyProgramDto), "Weekly")]
public abstract record ComponentProgramDto(Ulid Id)
{
    public T Match<T>(
        Func<ManualProgramDto, T> mapManual,
        Func<CycleProgramDto, T> mapCycle,
        Func<DailyProgramDto, T> mapDaily,
        Func<WeeklyProgramDto, T> mapWeekly) =>
        this switch
        {
            ManualProgramDto program => mapManual(program),
            CycleProgramDto program => mapCycle(program),
            DailyProgramDto program => mapDaily(program),
            WeeklyProgramDto program => mapWeekly(program),
            _ => throw new InvalidOperationException()
        };
}
