using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class IntervalExtensions
{
    public static IntervalDb ToDb(this TimeInterval<TimeOnlyWrapper> interval) => new()
    {
        Start = interval.Start.Inner.ToShortTimeString(),
        End = interval.End.Inner.ToShortTimeString()
    };
    
    public static Fin<TimeInterval<TimeOnlyWrapper>> ToDailyEntry(this IntervalDb interval) => (
        from start in interval.Start.ToTimeOnly()
        from end in interval.End.ToTimeOnly()
        select new TimeInterval<TimeOnlyWrapper>(new TimeOnlyWrapper(start), new TimeOnlyWrapper(end)))
    .MapFail(error => Error.New("Failed to parse daily time interval", error));
    
    public static Fin<TimeOnly> ToTimeOnly(this string time) =>
        TimeOnly.TryParse(time, out TimeOnly result)
            ? result
            : Fin.Fail<TimeOnly>($"Failed to parse time: {time}");

    public static IntervalDb ToDb(this TimeInterval<WeekTimeOnly> interval) => new()
    {
        Start = interval.Start.ToDb(),
        End = interval.End.ToDb()
    };

    public static Fin<TimeInterval<WeekTimeOnly>> ToWeeklyEntry(this IntervalDb interval) => (
            from start in interval.Start.ToWeekTimeOnly()
            from end in interval.End.ToWeekTimeOnly()
            select new TimeInterval<WeekTimeOnly>(start, end))
        .MapFail(error => Error.New("Failed to parse weekly time interval", error));

    public static string ToDb(this WeekTimeOnly time) => $"{time.DayOfWeek} {time.Time.ToShortTimeString()}";
    
    public static Fin<WeekTimeOnly> ToWeekTimeOnly(this string time)
    {
        string[] parts = time.Split(' ');
        
        if (parts.Length is not 2)
            return Fin.Fail<WeekTimeOnly>("WeekTimeOnly format must contains 2 parts separated by space");
        
        if (!Enum.TryParse(parts[0], out DayOfWeek dayOfWeek))
            return Fin.Fail<WeekTimeOnly>($"Failed to parse day of week: {parts[0]}");
        
        if (!TimeOnly.TryParse(parts[1], out TimeOnly timeOnly))
            return Fin.Fail<WeekTimeOnly>($"Failed to parse time: {parts[1]}");

        return new WeekTimeOnly(dayOfWeek, timeOnly);
    }
}