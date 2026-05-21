using System.Numerics;

namespace SmartGrowHub.Domain.Common;

public readonly record struct WeekTimeOnly(DayOfWeek DayOfWeek, TimeOnly TimeOnly) :
    IComparable<WeekTimeOnly>,
    IComparisonOperators<WeekTimeOnly, WeekTimeOnly, bool>,
    ISubtractionOperators<WeekTimeOnly, WeekTimeOnly, TimeSpan>
{
    public int CompareTo(WeekTimeOnly other)
    {
        int weekResult = DayOfWeek.CompareTo(other.DayOfWeek);
        return weekResult is 0 ? TimeOnly.CompareTo(other.TimeOnly) : weekResult;
    }

    public static bool operator >(WeekTimeOnly left, WeekTimeOnly right) => left.CompareTo(right) > 0;
    public static bool operator >=(WeekTimeOnly left, WeekTimeOnly right) => left.CompareTo(right) >= 0;
    public static bool operator <(WeekTimeOnly left, WeekTimeOnly right) => left.CompareTo(right) < 0;
    public static bool operator <=(WeekTimeOnly left, WeekTimeOnly right) => left.CompareTo(right) <= 0;

    public static TimeSpan operator -(WeekTimeOnly left, WeekTimeOnly right)
    {
        TimeSpan daysDelta = TimeSpan.FromDays(left.DayOfWeek - right.DayOfWeek);
        if (daysDelta.Days < 0) daysDelta += TimeSpan.FromDays(7);
        return daysDelta.Add(left.TimeOnly - right.TimeOnly);
    }
}