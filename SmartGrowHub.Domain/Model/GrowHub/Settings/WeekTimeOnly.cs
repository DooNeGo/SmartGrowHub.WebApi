namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public readonly record struct WeekTimeOnly(DayOfWeek DayOfWeek, TimeOnly TimeOnly) : IOperations<WeekTimeOnly, TimeSpan>
{
    public static bool operator >(WeekTimeOnly left, WeekTimeOnly right) =>
        left.DayOfWeek > right.DayOfWeek || left.DayOfWeek == right.DayOfWeek && left.TimeOnly > right.TimeOnly;

    public static bool operator <(WeekTimeOnly left, WeekTimeOnly right) => !(left > right);
    
    public static bool operator >=(WeekTimeOnly left, WeekTimeOnly right) =>
        left.DayOfWeek > right.DayOfWeek || left.DayOfWeek == right.DayOfWeek && left.TimeOnly >= right.TimeOnly;

    public static bool operator <=(WeekTimeOnly left, WeekTimeOnly right) => !(left >= right);
    
    public static TimeSpan operator -(WeekTimeOnly left, WeekTimeOnly right) =>
        TimeSpan.FromDays(left.DayOfWeek - right.DayOfWeek).Add(left.TimeOnly - right.TimeOnly);

    public TimeSpan Subtract(WeekTimeOnly other) => this - other;
    
    public bool IsGreaterThan(WeekTimeOnly other) => this > other;

    public bool IsLessThan(WeekTimeOnly other) => this < other;

    public bool IsGreaterThanOrEqual(WeekTimeOnly other) => this >= other;

    public bool IsLessThanOrEqual(WeekTimeOnly other) => this <= other;
}