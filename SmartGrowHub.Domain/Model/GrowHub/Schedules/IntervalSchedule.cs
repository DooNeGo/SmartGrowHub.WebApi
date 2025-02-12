using System.Collections.Immutable;
using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Schedules;

public abstract class IntervalSchedule<TTime, TSchedule>(
    Id<TSchedule> id, ImmutableArray<ValueWithInterval<TTime>> intervals) : Entity<TSchedule>(id)
    where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan>
    where TSchedule : IntervalSchedule<TTime, TSchedule>
{
    public ImmutableArray<ValueWithInterval<TTime>> Intervals { get; init; } = intervals;

    protected static Fin<TSchedule> ValidateAndCreate(
        ImmutableArray<ValueWithInterval<TTime>> intervals,
        Func<ImmutableArray<ValueWithInterval<TTime>>, Fin<TSchedule>> factory) =>
        intervals.HasOverlappingIntervals() 
            ? Error.New("Intervals must not overlap")
            : factory(intervals);
}