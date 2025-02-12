using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Schedules;

public sealed class WeeklySchedule(Id<WeeklySchedule> id, ImmutableArray<ValueWithInterval<WeekTimeOnly>> intervals)
    : IntervalSchedule<WeekTimeOnly, WeeklySchedule>(id, intervals)
{
    private const int DaysInWeek = 7;
    
    public static Fin<WeeklySchedule> New(ImmutableArray<ValueWithInterval<WeekTimeOnly>> intervals) =>
        ValidateAndCreate(intervals, static array =>
            array.ToTimeInterval().Duration > TimeSpan.FromDays(DaysInWeek)
                ? Error.New($"Duration must be less than or equal {DaysInWeek} days")
                : new WeeklySchedule(new Id<WeeklySchedule>(), array));
}