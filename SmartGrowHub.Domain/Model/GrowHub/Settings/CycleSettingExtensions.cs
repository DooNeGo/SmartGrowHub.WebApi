using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public static class CycleSettingExtensions
{
    public static TimeSpan GetTotalDuration<TTime>(this ImmutableArray<CycleSetting<TTime>> settings)
        where TTime : IOperations<TTime, TimeSpan> =>
        settings.Aggregate(TimeSpan.Zero, (timeSpan, setting) => timeSpan + setting.TimePeriod.Duration);

    public static TimePeriod<TTime> ToTimePeriod<TTime>(this ImmutableArray<CycleSetting<TTime>> settings)
        where TTime : IOperations<TTime, TimeSpan> =>
        new(settings.GetStartTime(), settings.GetEndTime());
    
    public static TTime GetStartTime<TTime>(this ImmutableArray<CycleSetting<TTime>> settings)
        where TTime : IOperations<TTime, TimeSpan> =>
        settings.Select(setting => setting.TimePeriod.Start)
            .Aggregate((startTime1, startTime2) => startTime1.IsLessThan(startTime2) ? startTime1 : startTime2);

    public static TTime GetEndTime<TTime>(this ImmutableArray<CycleSetting<TTime>> settings)
        where TTime : IOperations<TTime, TimeSpan> =>
        settings.Select(setting => setting.TimePeriod.End)
            .Aggregate((endTime1, endTime2) => endTime1.IsGreaterThan(endTime2) ? endTime1 : endTime2);

    public static bool HasOverlappingPeriods<TTime>(this ImmutableArray<CycleSetting<TTime>> settings)
        where TTime : IOperations<TTime, TimeSpan>
    {
        ReadOnlySpan<CycleSetting<TTime>> span = settings.AsSpan();
        
        for (var i = 0; i < span.Length; i++)
        {
            TimePeriod<TTime> current = span[i].TimePeriod;

            for (int j = i + 1; j < span.Length - 1; j++)
            {
                TimePeriod<TTime> other = span[j].TimePeriod;
                if (current.IsOverlaps(other)) return true;
            }
        }

        return false;
    }
}