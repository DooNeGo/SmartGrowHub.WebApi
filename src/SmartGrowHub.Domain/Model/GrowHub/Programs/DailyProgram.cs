using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using TimedQuantityCollection = System.Collections.Immutable.ImmutableArray<SmartGrowHub.Domain.Model.GrowHub.Programs.TimedQuantity<SmartGrowHub.Domain.Common.TimeOnlyWrapper>>;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public sealed class DailyProgram : ModuleProgram
{
    private DailyProgram(Id<ModuleProgram> id, TimedQuantityCollection entries) : base(id) =>
        Entries = entries;

    public TimedQuantityCollection Entries { get; }

    public static Fin<DailyProgram> New(TimedQuantityCollection entries, Id<ModuleProgram>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromHours(TimeSpan.HoursPerDay))
            return Error.New($"Duration must be less than or equal {TimeSpan.HoursPerDay} hours");

        return new DailyProgram(id ?? new Id<ModuleProgram>(), entries);
    }
}