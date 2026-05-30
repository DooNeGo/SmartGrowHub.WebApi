using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.Programs;

public abstract class ModuleProgram(Id<ModuleProgram> id) : Entity<ModuleProgram>(id)
{
    public T Match<T>(
        Func<DisabledProgram, T> mapDisable,
        Func<ManualProgram, T> mapManual,
        Func<DailyProgram, T> mapDaily,
        Func<WeeklyProgram, T> mapWeekly) =>
        this switch
        {
            DisabledProgram program => mapDisable(program),
            ManualProgram program => mapManual(program),
            DailyProgram program => mapDaily(program),
            WeeklyProgram program => mapWeekly(program),
            _ => throw new InvalidOperationException()
        };
}