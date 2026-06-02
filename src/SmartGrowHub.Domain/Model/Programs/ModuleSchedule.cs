using System.Diagnostics.CodeAnalysis;
using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.Programs;

public abstract class ModuleSchedule : Entity<ModuleSchedule>
{
    protected ModuleSchedule(Id<ModuleSchedule> id, Id<GrowHubModule> moduleId) : base(id) => ModuleId = moduleId;

    public Id<GrowHubModule> ModuleId { get; init; }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public T Match<T>(
        Func<DisabledSchedule, T> Disabled,
        Func<EnabledSchedule, T> Enabled,
        Func<DailySchedule, T> Daily,
        Func<WeeklySchedule, T> Weekly) =>
        this switch
        {
            DisabledSchedule schedule => Disabled(schedule),
            EnabledSchedule schedule => Enabled(schedule),
            DailySchedule schedule => Daily(schedule),
            WeeklySchedule schedule => Weekly(schedule),
            _ => throw new InvalidOperationException()
        };
}