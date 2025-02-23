using System.Numerics;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Extensions;

public static class TimedQuantityExtensions
{
    public static TimeInterval<TTime> CalculateTimeInterval<TTime>(
        this IEnumerable<TimedQuantity<TTime>> timedQuantities)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan> =>
        timedQuantities.Select(timed => timed.TimeInterval).CalculateTimeInterval();

    public static TTime GetStartTime<TTime>(this IEnumerable<TimedQuantity<TTime>> timedQuantities)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan> =>
        timedQuantities
            .Select(timedQuantity => timedQuantity.TimeInterval)
            .GetStartTime();

    public static TTime GetEndTime<TTime>(this IEnumerable<TimedQuantity<TTime>> timedQuantities)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan> =>
        timedQuantities
            .Select(timedQuantity => timedQuantity.TimeInterval)
            .GetEndTime();
    
    public static bool HasOverlappingIntervals<TTime>(this IEnumerable<TimedQuantity<TTime>> timedQuantities)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan> =>
        timedQuantities
            .Select(timedQuantity => timedQuantity.TimeInterval)
            .GetUniquePairs()
            .Any(pair => pair.First.IsOverlaps(pair.Second));
}