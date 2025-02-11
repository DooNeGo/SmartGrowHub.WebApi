namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public abstract record Setting
{
    public T Match<T>(
        Func<ManualSetting, T> mapManual,
        Func<CycleSetting<TimeOnlyWrapper>, T> mapCycle,
        Func<DayScheduleSetting, T> mapDaySchedule,
        Func<WeekScheduleSetting, T> mapWeekSchedule) =>
        this switch
        {
            ManualSetting manual => mapManual(manual),
            CycleSetting<TimeOnlyWrapper> cycle => mapCycle(cycle),
            DayScheduleSetting daySchedule => mapDaySchedule(daySchedule),
            WeekScheduleSetting weekSchedule => mapWeekSchedule(weekSchedule),
            _ => throw new InvalidOperationException()
        };
}
