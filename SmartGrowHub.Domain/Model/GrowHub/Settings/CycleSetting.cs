using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed class CycleSetting(Id<Setting> id, ValueWithInterval<TimeOnlyWrapper> interval) : Setting(id)
{
    public ValueWithInterval<TimeOnlyWrapper> Interval { get; } = interval;
    
    public static CycleSetting New(ValueWithInterval<TimeOnlyWrapper> interval) => new(new Id<Setting>(), interval);
}