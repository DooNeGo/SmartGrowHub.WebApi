using System.Numerics;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Extensions;

public static class TimedQuantityExtensions
{
    extension<TTime>(IEnumerable<TimedQuantity<TTime>> timedQuantities)
        where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan>
    {
        public TimeInterval<TTime> CalculateTimeInterval() =>
            timedQuantities.Select(timed => timed.TimeInterval).CalculateTimeInterval();

        public TTime GetStartTime() =>
            timedQuantities
                .Select(timedQuantity => timedQuantity.TimeInterval)
                .GetStartTime();

        public TTime GetEndTime() =>
            timedQuantities
                .Select(timedQuantity => timedQuantity.TimeInterval)
                .GetEndTime();

        public bool HasOverlappingIntervals() =>
            timedQuantities
                .Select(timedQuantity => timedQuantity.TimeInterval)
                .GetUniquePairs()
                .Any(pair => pair.First.IsOverlaps(pair.Second));
    }
}