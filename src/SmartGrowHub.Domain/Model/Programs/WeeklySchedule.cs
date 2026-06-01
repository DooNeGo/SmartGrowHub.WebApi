using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class WeeklySchedule : ModuleSchedule
{
    private WeeklySchedule(
        Id<ModuleSchedule> id,
        Id<GrowHubModule> moduleId,
        ImmutableList<ScheduleUnit<WeekTimeOnly>> entries) : base(id, moduleId) =>
        Entries = entries;

    public ImmutableList<ScheduleUnit<WeekTimeOnly>> Entries { get; }

    public static Fin<WeeklySchedule> New(ImmutableList<ScheduleUnit<WeekTimeOnly>> entries, Id<GrowHubModule> moduleId,
        Id<ModuleSchedule>? id = null) =>
        entries.HasOverlappingIntervals()
            ? Error.New("Intervals must not overlap")
            : new WeeklySchedule(id ?? new Id<ModuleSchedule>(), moduleId, entries);
}