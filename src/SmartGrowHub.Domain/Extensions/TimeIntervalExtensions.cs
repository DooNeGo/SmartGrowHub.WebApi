using System.Numerics;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Extensions;

public static class TimeIntervalExtensions
{
    public static TimeInterval<TTime> CalculateTimeInterval<TTime>(this IEnumerable<TimeInterval<TTime>> timeIntervals)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan>
    {
        TimeInterval<TTime>[] array = [.. timeIntervals];
        return new TimeInterval<TTime>(array.GetStartTime(), array.GetEndTime());
    }

    public static TTime GetStartTime<TTime>(this IEnumerable<TimeInterval<TTime>> timeIntervals)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan> =>
        timeIntervals
            .Select(timeInterval => timeInterval.Start)
            .Aggregate((start1, start2) => start1 < start2 ? start1 : start2);
    
    public static TTime GetEndTime<TTime>(this IEnumerable<TimeInterval<TTime>> timeIntervals)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan> =>
        timeIntervals
            .Select(timeInterval => timeInterval.End)
            .Aggregate((end1, end2) => end1 > end2 ? end1 : end2);
}