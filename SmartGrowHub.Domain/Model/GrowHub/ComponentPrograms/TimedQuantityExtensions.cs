using System.Collections.Immutable;
using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

public static class TimedQuantityExtensions
{
    public static TimeInterval<TTime> ToTimeInterval<TTime>(this ImmutableArray<TimedQuantity<TTime>> settings)
        where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan> =>
        new(settings.GetStartTime(), settings.GetEndTime());
    
    public static TTime GetStartTime<TTime>(this ImmutableArray<TimedQuantity<TTime>> settings)
        where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan> =>
        settings.Select(setting => setting.TimeInterval.Start)
            .Aggregate((startTime1, startTime2) => startTime1.CompareTo(startTime2) < 0 ? startTime1 : startTime2);

    public static TTime GetEndTime<TTime>(this ImmutableArray<TimedQuantity<TTime>> settings)
        where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan> =>
        settings.Select(setting => setting.TimeInterval.End)
            .Aggregate((endTime1, endTime2) => endTime1.CompareTo(endTime2) > 0 ? endTime1 : endTime2);

    public static bool HasOverlappingIntervals<TTime>(this ImmutableArray<TimedQuantity<TTime>> settings)
        where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan>
    {
        ReadOnlySpan<TimedQuantity<TTime>> span = settings.AsSpan();
        
        for (var i = 0; i < span.Length - 1; i++)
        {
            TimeInterval<TTime> current = span[i].TimeInterval;

            for (int j = i + 1; j < span.Length; j++)
            {
                TimeInterval<TTime> other = span[j].TimeInterval;
                if (current.IsOverlaps(other)) return true;
            }
        }

        return false;
    }
}