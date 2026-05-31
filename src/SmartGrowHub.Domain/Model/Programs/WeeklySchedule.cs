using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class WeeklySchedule : ModuleSchedule
{
    private const int DaysInWeek = 7;

    private WeeklySchedule(
        Id<ModuleSchedule> id,
        Id<GrowHubModule> moduleId,
        ImmutableList<ScheduleUnit<WeekTimeOnly>> entries) : base(id, moduleId) =>
        Entries = entries;

    public ImmutableList<ScheduleUnit<WeekTimeOnly>> Entries { get; }

    public static Fin<WeeklySchedule> New(ImmutableList<ScheduleUnit<WeekTimeOnly>> entries, Id<GrowHubModule> moduleId,
        Id<ModuleSchedule>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromDays(DaysInWeek))
            return Error.New($"Duration must be less than or equal {DaysInWeek} days");

        return new WeeklySchedule(id ?? new Id<ModuleSchedule>(), moduleId, entries);
    }
}