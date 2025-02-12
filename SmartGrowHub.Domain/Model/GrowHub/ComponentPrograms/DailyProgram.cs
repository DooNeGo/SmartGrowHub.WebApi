using SmartGrowHub.Domain.Common;
using TimedQuantityCollection = System.Collections.Immutable.ImmutableArray<SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms.TimedQuantity<SmartGrowHub.Domain.Common.TimeOnlyWrapper>>;

namespace SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

public sealed class DailyProgram : ComponentProgram
{
    private DailyProgram(Id<ComponentProgram> id, TimedQuantityCollection entries) : base(id) =>
        Entries = entries;

    public TimedQuantityCollection Entries { get; }

    public static Fin<DailyProgram> New(TimedQuantityCollection entries, Id<ComponentProgram>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.ToTimeInterval().Duration > TimeSpan.FromHours(TimeSpan.HoursPerDay))
            return Error.New($"Duration must be less than or equal {TimeSpan.HoursPerDay} hours");

        return new DailyProgram(id ?? new Id<ComponentProgram>(), entries);
    }
}