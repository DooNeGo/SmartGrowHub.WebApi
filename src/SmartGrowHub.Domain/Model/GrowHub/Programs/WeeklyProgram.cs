using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using WeekTimedQuantityCollection = System.Collections.Immutable.ImmutableArray<SmartGrowHub.Domain.Model.GrowHub.Programs.TimedQuantity<SmartGrowHub.Domain.Common.WeekTimeOnly>>;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public sealed class WeeklyProgram(Id<ModuleProgram> id, WeekTimedQuantityCollection entries)
    : ModuleProgram(id)
{
    private const int DaysInWeek = 7;
    
    public WeekTimedQuantityCollection Entries { get; } = entries;

    public static Fin<WeeklyProgram> New(WeekTimedQuantityCollection entries, Id<ModuleProgram>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromDays(DaysInWeek))
            return Error.New($"Duration must be less than or equal {DaysInWeek} days");
        
        return new WeeklyProgram(id ?? new Id<ModuleProgram>(), entries);
    }
}