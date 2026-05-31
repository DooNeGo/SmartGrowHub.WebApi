using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.Programs;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

public static class GrowHubExtensions
{
    public static ScheduleUnitDto<WeekTimeOnlyDto> ToDto(this ScheduleUnit<WeekTimeOnly> scheduleUnit) => new(
        scheduleUnit.Id,
        scheduleUnit.Kind.ToDto(),
        scheduleUnit.Quantity.ToDto(),
        scheduleUnit.TimeInterval.ToDto());

    public static Fin<ScheduleUnit<WeekTimeOnly>> ToDomain(this ScheduleUnitDto<WeekTimeOnlyDto> scheduleUnit,
        Id<ModuleSchedule> scheduleId) =>
        from id in Domain.Common.Id<ScheduleUnit<WeekTimeOnly>>.From(scheduleUnit.Id)
        from unit in ScheduleUnit<WeekTimeOnly>.New(
            scheduleId,
            scheduleUnit.Kind.ToDomain(),
            scheduleUnit.Quantity.ToDomain(),
            scheduleUnit.Interval.ToDomain(), id)
        select unit;

    public static ScheduleUnitDto<TimeOnly> ToDto(this ScheduleUnit<TimeOnlyWrapper> scheduleUnit) => new(
        scheduleUnit.Id,
        scheduleUnit.Kind.ToDto(),
        scheduleUnit.Quantity.ToDto(),
        scheduleUnit.TimeInterval.ToDto());

    public static Fin<ScheduleUnit<TimeOnlyWrapper>> ToDomain(this ScheduleUnitDto<TimeOnly> scheduleUnit,
        Id<ModuleSchedule> scheduleId) =>
        from id in Domain.Common.Id<ScheduleUnit<TimeOnlyWrapper>>.From(scheduleUnit.Id)
        from unit in ScheduleUnit<TimeOnlyWrapper>.New(
            scheduleId,
            scheduleUnit.Kind.ToDomain(),
            scheduleUnit.Quantity.ToDomain(),
            scheduleUnit.Interval.ToDomain(), id)
        select unit;
    
    public static TimeIntervalDto<TimeOnly> ToDto(this TimeInterval<TimeOnlyWrapper> interval) =>
        new(interval.Start.ToDto(), interval.End.ToDto());
    
    public static TimeInterval<TimeOnlyWrapper> ToDomain(this TimeIntervalDto<TimeOnly> interval) =>
        new(interval.Start.ToDomain(), interval.End.ToDomain());
    
    public static TimeIntervalDto<WeekTimeOnlyDto> ToDto(this TimeInterval<WeekTimeOnly> interval) =>
        new(interval.Start.ToDto(), interval.End.ToDto());
    
    public static TimeInterval<WeekTimeOnly> ToDomain(this TimeIntervalDto<WeekTimeOnlyDto> interval) =>
        new(interval.Start.ToDomain(), interval.End.ToDomain());
    
    public static TimeOnly ToDto(this TimeOnlyWrapper time) => time.Inner;
    
    public static TimeOnlyWrapper ToDomain(this TimeOnly time) => new(time);
    
    public static WeekTimeOnlyDto ToDto(this WeekTimeOnly time) => new(time.DayOfWeek, time.Time);
    
    public static WeekTimeOnly ToDomain(this WeekTimeOnlyDto time) => new(time.DayOfWeek, time.Time);
    
    public static QuantityDto ToDto(this Quantity value) => new(value.Magnitude, value.Unit.ToDto());

    public static Quantity ToDomain(this QuantityDto quantity) =>
        new(quantity.Magnitude, quantity.Unit.ToMeasurementUnit());
    
    public static ScheduleUnitKindDto ToDto(this ScheduleUnitKind kind) => kind switch
    {
        ScheduleUnitKind.Power => ScheduleUnitKindDto.Power,
        ScheduleUnitKind.Prefer => ScheduleUnitKindDto.Prefer,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };
    
    public static ScheduleUnitKind ToDomain(this ScheduleUnitKindDto kind) => kind switch
    {
        ScheduleUnitKindDto.Power => ScheduleUnitKind.Power,
        ScheduleUnitKindDto.Prefer => ScheduleUnitKind.Prefer,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };
    
    public static string ToDto(this MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Celsius => "\u00b0C",
        MeasurementUnit.Percent => "%",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };

    public static MeasurementUnit ToMeasurementUnit(this string unit) => unit switch
    {
        "\u00b0C" => MeasurementUnit.Celsius,
        "%" => MeasurementUnit.Percent,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}