using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public abstract record ScheduleSetting<TTime>(ImmutableArray<CycleSetting<TTime>> Schedules)
    : Setting where TTime : IOperations<TTime, TimeSpan>
{
    public ImmutableArray<CycleSetting<TTime>> Schedules { get; init; } = Schedules;
    
    public TimePeriod<TTime> TimePeriod { get; init; } = Schedules.ToTimePeriod();
}