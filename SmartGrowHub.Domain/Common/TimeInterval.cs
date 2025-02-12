using SmartGrowHub.Domain.Abstractions;

namespace SmartGrowHub.Domain.Common;

public readonly record struct TimeInterval<TTime>(TTime Start, TTime End)
    where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan>
{
    public TimeSpan Duration => End.Subtract(Start);
    
    public bool IsCrossover => Start.CompareTo(End) > 0;
    
    public bool IsOverlaps(TimeInterval<TTime> other) =>
        Contains(other.Start) || Contains(other.End) || other.Contains(Start) || other.Contains(End);

    public bool Contains(TTime time) => IsCrossover ? IsWithinWrappedInterval(time) : IsWithinNormalInterval(time);

    private bool IsWithinNormalInterval(TTime time) => time.CompareTo(Start) > 0 && time.CompareTo(End) < 0;
    
    private bool IsWithinWrappedInterval(TTime time) => time.CompareTo(Start) > 0 || time.CompareTo(End) < 0;
}