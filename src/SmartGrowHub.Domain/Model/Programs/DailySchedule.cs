using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class DailySchedule : ModuleSchedule
{
    private DailySchedule(
        Id<ModuleSchedule> id,
        Id<GrowHubModule> moduleId,
        ImmutableList<ScheduleUnit<TimeOnlyWrapper>> entries) : base(id, moduleId) =>
        Entries = entries;

    public ImmutableList<ScheduleUnit<TimeOnlyWrapper>> Entries { get; }

    public static Fin<DailySchedule> New(ImmutableList<ScheduleUnit<TimeOnlyWrapper>> entries,
        Id<GrowHubModule> moduleId, Id<ModuleSchedule>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromHours(TimeSpan.HoursPerDay))
            return Error.New($"Duration must be less than or equal {TimeSpan.HoursPerDay} hours");

        return new DailySchedule(id ?? new Id<ModuleSchedule>(), moduleId, entries);
    }
}