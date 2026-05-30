using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

public static class GrowHubExtensions
{
    public static TimedQuantityDto<WeekTimeOnlyDto> ToDto(this TimedQuantity<WeekTimeOnly> interval) =>
        new(interval.Quantity.ToDto(), interval.TimeInterval.ToDto());
    
    public static TimedQuantity<WeekTimeOnly> ToDomain(this TimedQuantityDto<WeekTimeOnlyDto> interval) =>
        new(interval.Quantity.ToDomain(), interval.Interval.ToDomain());
    
    public static TimedQuantityDto<TimeOnly> ToDto(this TimedQuantity<TimeOnlyWrapper> interval) =>
        new(interval.Quantity.ToDto(), interval.TimeInterval.ToDto());
    
    public static TimedQuantity<TimeOnlyWrapper> ToDomain(this TimedQuantityDto<TimeOnly> interval) =>
        new(interval.Quantity.ToDomain(), interval.Interval.ToDomain());
    
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