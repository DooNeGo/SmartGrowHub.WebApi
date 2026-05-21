using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using WeekTimedQuantityCollection = System.Collections.Immutable.ImmutableArray<SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms.TimedQuantity<SmartGrowHub.Domain.Common.WeekTimeOnly>>;

namespace SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

public sealed class WeeklyProgram(Id<ComponentProgram> id, WeekTimedQuantityCollection entries)
    : ComponentProgram(id)
{
    private const int DaysInWeek = 7;
    
    public WeekTimedQuantityCollection Entries { get; } = entries;

    public static Fin<WeeklyProgram> New(WeekTimedQuantityCollection entries, Id<ComponentProgram>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromDays(DaysInWeek))
            return Error.New($"Duration must be less than or equal {DaysInWeek} days");
        
        return new WeeklyProgram(id ?? new Id<ComponentProgram>(), entries);
    }
}