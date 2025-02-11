namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public readonly record struct TimePeriod<TTime>(TTime Start, TTime End) where TTime : IOperations<TTime, TimeSpan>
{
    public TimeSpan Duration { get; } = End.Subtract(Start);
    
    public bool IsOverlaps(TimePeriod<TTime> other) =>
        Start.IsGreaterThanOrEqual(other.Start) && Start.IsLessThan(other.End) ||
        End.IsGreaterThan(other.Start) && End.IsLessThanOrEqual(other.End);
}