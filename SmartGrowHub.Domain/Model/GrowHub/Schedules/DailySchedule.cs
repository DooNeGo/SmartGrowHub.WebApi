using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Schedules;

public sealed class DailySchedule(Id<DailySchedule> id, ImmutableArray<ValueWithInterval<TimeOnlyWrapper>> intervals)
    : IntervalSchedule<TimeOnlyWrapper, DailySchedule>(id, intervals)
{
    public static Fin<DailySchedule> New(ImmutableArray<ValueWithInterval<TimeOnlyWrapper>> intervals) =>
        ValidateAndCreate(intervals, array =>
            array.ToTimeInterval().Duration > TimeSpan.FromHours(TimeSpan.HoursPerDay)
                ? Error.New($"Duration must be less than or equal {TimeSpan.HoursPerDay} hours")
                : new DailySchedule(new Id<DailySchedule>(), array));
}