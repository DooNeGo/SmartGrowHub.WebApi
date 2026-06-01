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
        Id<GrowHubModule> moduleId, Id<ModuleSchedule>? id = null) =>
        entries.HasOverlappingIntervals()
            ? Error.New("Intervals must not overlap")
            : new DailySchedule(id ?? new Id<ModuleSchedule>(), moduleId, entries);
}