using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed record DayScheduleSetting(ImmutableArray<CycleSetting<TimeOnlyWrapper>> Schedules)
    : ScheduleSetting<TimeOnlyWrapper>(Schedules)
{
    public static Fin<DayScheduleSetting> New(ImmutableArray<CycleSetting<TimeOnlyWrapper>> schedules)
    {
        TimeSpan duration = schedules.GetTotalDuration();
        
        if (duration > TimeSpan.FromHours(TimeSpan.HoursPerDay))
            return Error.New("Duration must be less than or equal 24 hours");

        return schedules.HasOverlappingPeriods()
            ? Error.New("Schedules must not overlap")
            : new DayScheduleSetting(schedules);
    }
}