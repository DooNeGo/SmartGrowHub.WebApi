using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed record WeekScheduleSetting(ImmutableArray<CycleSetting<WeekTimeOnly>> Schedules)
    : ScheduleSetting<WeekTimeOnly>(Schedules)
{
    private const int DaysInWeek = 7;

    public static Fin<WeekScheduleSetting> New(ImmutableArray<CycleSetting<WeekTimeOnly>> schedules)
    {
        TimeSpan duration = schedules.GetTotalDuration();
        
        if (duration >= TimeSpan.FromDays(DaysInWeek))
            return Error.New($"Duration must be less than {DaysInWeek} days");
        
        if (schedules.HasOverlappingPeriods()) return Error.New("Schedules must not overlap");
        
        return new WeekScheduleSetting(schedules);
    }
}