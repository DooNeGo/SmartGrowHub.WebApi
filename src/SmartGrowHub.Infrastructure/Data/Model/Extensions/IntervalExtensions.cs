using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class IntervalExtensions
{
    public static IntervalDb<TimeOnly> ToDb(this TimeInterval<TimeOnlyWrapper> interval) => new()
    {
        Start = interval.Start.Inner,
        End = interval.End.Inner
    };
    
    public static TimeInterval<TimeOnlyWrapper> ToDomain(this IntervalDb<TimeOnly> interval) =>
        new(new TimeOnlyWrapper(interval.Start), new TimeOnlyWrapper(interval.End));

    public static IntervalDb<WeekTimeOnlyDb> ToDb(this TimeInterval<WeekTimeOnly> interval) => new()
    {
        Start = new WeekTimeOnlyDb(interval.Start.DayOfWeek, interval.Start.Time),
        End = new WeekTimeOnlyDb(interval.End.DayOfWeek, interval.End.Time)
    };
    
    public static TimeInterval<WeekTimeOnly> ToDomain(this IntervalDb<WeekTimeOnlyDb> interval) => new(
        new WeekTimeOnly(interval.Start.DayOfWeek, interval.Start.TimeOnly),
        new WeekTimeOnly(interval.End.DayOfWeek, interval.End.TimeOnly));
}