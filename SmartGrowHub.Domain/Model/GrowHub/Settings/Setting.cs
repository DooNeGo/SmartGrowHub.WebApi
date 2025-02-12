using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public abstract class Setting(Id<Setting> id) : Entity<Setting>(id)
{
    public T Match<T>(
        Func<ManualSetting, T> mapManual,
        Func<CycleSetting, T> mapCycle,
        Func<DailySetting, T> mapDaily,
        Func<WeeklySetting, T> mapWeekly) =>
        this switch
        {
            ManualSetting setting => mapManual(setting),
            CycleSetting setting => mapCycle(setting),
            DailySetting setting => mapDaily(setting),
            WeeklySetting setting => mapWeekly(setting),
            _ => throw new InvalidOperationException()
        };
}