using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.Programs;

public abstract class ModuleSchedule : Entity<ModuleSchedule>
{
    protected ModuleSchedule(Id<ModuleSchedule> id, Id<GrowHubModule> moduleId) : base(id) => ModuleId = moduleId;

    public Id<GrowHubModule> ModuleId { get; init; }
    
    public T Match<T>(
        Func<DisabledSchedule, T> mapDisable,
        Func<EnabledSchedule, T> mapEnabled,
        Func<DailySchedule, T> mapDaily,
        Func<WeeklySchedule, T> mapWeekly) =>
        this switch
        {
            DisabledSchedule schedule => mapDisable(schedule),
            EnabledSchedule schedule => mapEnabled(schedule),
            DailySchedule schedule => mapDaily(schedule),
            WeeklySchedule schedule => mapWeekly(schedule),
            _ => throw new InvalidOperationException()
        };
}