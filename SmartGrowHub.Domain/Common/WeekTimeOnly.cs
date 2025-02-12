using SmartGrowHub.Domain.Abstractions;

namespace SmartGrowHub.Domain.Common;

public readonly record struct WeekTimeOnly(DayOfWeek DayOfWeek, TimeOnly TimeOnly)
    : IComparable<WeekTimeOnly>, IArithmetic<WeekTimeOnly, TimeSpan>
{
    public int CompareTo(WeekTimeOnly other)
    {
        int weekResult = DayOfWeek.CompareTo(other.DayOfWeek);
        return weekResult is 0 ? TimeOnly.CompareTo(other.TimeOnly) : weekResult;
    }

    public TimeSpan Subtract(WeekTimeOnly other)
    {
        TimeSpan daysDelta = TimeSpan.FromDays(DayOfWeek - other.DayOfWeek);
        if (daysDelta.Days < 0) daysDelta += TimeSpan.FromDays(7);
        return daysDelta.Add(TimeOnly - other.TimeOnly);
    }
}