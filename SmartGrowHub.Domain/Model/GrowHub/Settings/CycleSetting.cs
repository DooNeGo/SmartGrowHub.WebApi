namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed record CycleSetting<TTime> : Setting
    where TTime : IOperations<TTime, TimeSpan>
{
    private CycleSetting(TimePeriod<TTime> period, SettingValue value) =>
        (TimePeriod, Value) = (period, value);

    public SettingValue Value { get; }
    
    public TimePeriod<TTime> TimePeriod { get; }

    public static Fin<CycleSetting<TTime>> New(SettingValue value, TimePeriod<TTime> period) =>
        period.Duration < TimeSpan.FromMinutes(1)
            ? Error.New("Duration must be at least 1 minute")
            : new CycleSetting<TTime>(period, value);
}