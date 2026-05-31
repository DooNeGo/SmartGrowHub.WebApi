using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class ScheduleUnitExtensions
{
    public static ScheduleUnitDb ToDb(this ScheduleUnit<TimeOnlyWrapper> scheduleUnit) => new()
    {
        Id = scheduleUnit.Id,
        Kind = scheduleUnit.Kind.ToDb(),
        Quantity = scheduleUnit.Quantity.ToDb(),
        Interval = scheduleUnit.TimeInterval.ToDb(),
        ScheduleId = scheduleUnit.ScheduleId
    };

    public static Fin<ScheduleUnit<TimeOnlyWrapper>> ToDailyScheduleUnit(this ScheduleUnitDb scheduleUnit) =>
        from id in Id<ScheduleUnit<TimeOnlyWrapper>>.From(scheduleUnit.Id)
        from scheduleId in Id<ModuleSchedule>.From(scheduleUnit.ScheduleId)
        from entry in scheduleUnit.Interval.ToDailyEntry()
        from domain in ScheduleUnit<TimeOnlyWrapper>.New(
            scheduleId,
            scheduleUnit.Kind.ToDomain(),
            scheduleUnit.Quantity.ToDomain(),
            entry, id)
        select domain;

    public static ScheduleUnitDb ToDb(this ScheduleUnit<WeekTimeOnly> scheduleUnit) => new()
    {
        Id = scheduleUnit.Id,
        Kind = scheduleUnit.Kind.ToDb(),
        Quantity = scheduleUnit.Quantity.ToDb(),
        Interval = scheduleUnit.TimeInterval.ToDb(),
        ScheduleId = scheduleUnit.ScheduleId
    };

    public static Fin<ScheduleUnit<WeekTimeOnly>> ToWeeklyScheduleUnit(this ScheduleUnitDb scheduleUnit) =>
        from id in Id<ScheduleUnit<WeekTimeOnly>>.From(scheduleUnit.Id)
        from scheduleId in Id<ModuleSchedule>.From(scheduleUnit.ScheduleId)
        from entry in scheduleUnit.Interval.ToWeeklyEntry()
        from domain in ScheduleUnit<WeekTimeOnly>.New(
            scheduleId,
            scheduleUnit.Kind.ToDomain(),
            scheduleUnit.Quantity.ToDomain(),
            entry, id)
        select domain;

    public static ScheduleUnitKindDb ToDb(this ScheduleUnitKind kind) => kind switch
    {
        ScheduleUnitKind.Power => ScheduleUnitKindDb.Power,
        ScheduleUnitKind.Prefer => ScheduleUnitKindDb.Prefer,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };

    public static ScheduleUnitKind ToDomain(this ScheduleUnitKindDb kind) => kind switch
    {
        ScheduleUnitKindDb.Power => ScheduleUnitKind.Power,
        ScheduleUnitKindDb.Prefer => ScheduleUnitKind.Prefer,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };
}