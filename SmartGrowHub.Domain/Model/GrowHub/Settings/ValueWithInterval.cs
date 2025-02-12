using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed class ValueWithInterval<TTime> : Entity<ValueWithInterval<TTime>>
    where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan>
{
    private ValueWithInterval(Id<ValueWithInterval<TTime>> id, TimeInterval<TTime> interval, SettingValue value)
        : base(id) => (TimeInterval, Value) = (interval, value);

    public SettingValue Value { get; }
    
    public TimeInterval<TTime> TimeInterval { get; }

    public static Fin<ValueWithInterval<TTime>> New(SettingValue value, TimeInterval<TTime> interval) =>
        interval.Duration < TimeSpan.FromMinutes(1)
            ? Error.New("Duration must be at least 1 minute")
            : new ValueWithInterval<TTime>(new Id<ValueWithInterval<TTime>>(), interval, value);
}