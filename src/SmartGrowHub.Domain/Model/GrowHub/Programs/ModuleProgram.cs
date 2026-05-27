using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public abstract class ModuleProgram(Id<ModuleProgram> id) : Entity<ModuleProgram>(id)
{
    public T Match<T>(
        Func<ManualProgram, T> mapManual,
        Func<CycleProgram, T> mapCycle,
        Func<DailyProgram, T> mapDaily,
        Func<WeeklyProgram, T> mapWeekly) =>
        this switch
        {
            ManualProgram program => mapManual(program),
            CycleProgram program => mapCycle(program),
            DailyProgram program => mapDaily(program),
            WeeklyProgram program => mapWeekly(program),
            _ => throw new InvalidOperationException()
        };
}