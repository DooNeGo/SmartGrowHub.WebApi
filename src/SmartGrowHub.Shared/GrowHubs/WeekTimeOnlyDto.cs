namespace SmartGrowHub.Shared.GrowHubs;

public readonly record struct WeekTimeOnlyDto(DayOfWeek DayOfWeek, TimeOnly Time) : IComparable<WeekTimeOnlyDto>
{
    public int CompareTo(WeekTimeOnlyDto other)
    {
        int weekResult = DayOfWeek.CompareTo(other.DayOfWeek);
        return weekResult is 0 ? Time.CompareTo(other.Time) : weekResult;
    }
}