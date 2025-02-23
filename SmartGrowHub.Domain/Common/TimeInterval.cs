using System.Numerics;

namespace SmartGrowHub.Domain.Common;

public readonly record struct TimeInterval<TTime>(TTime Start, TTime End)
    where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan>
{
    public TimeSpan Duration => End - Start;

    public bool IsCrossover => Start > End;

    public bool IsOverlaps(TimeInterval<TTime> other) =>
        Contains(other.Start) || Contains(other.End) || other.Contains(Start) || other.Contains(End);

    public bool Contains(TTime time) => IsCrossover ? IsWithinWrappedInterval(time) : IsWithinNormalInterval(time);

    private bool IsWithinNormalInterval(TTime time) => time > Start && time < End;

    private bool IsWithinWrappedInterval(TTime time) => time > Start || time < End;
}