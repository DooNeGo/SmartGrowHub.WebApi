namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed record CycleSetting : ISetting
{
    private CycleSetting(DateTime startTime, TimeSpan onDuration, int value)
    {
        StartTime = startTime;
        OnDuration = onDuration;
        Value = value;
    }

    public DateTime StartTime { get; }

    public TimeSpan OnDuration { get; }

    public int Value { get; }

    public static Fin<CycleSetting> From(DateTime startTime, TimeSpan onDuration, int value) =>
        onDuration > TimeSpan.FromHours(TimeSpan.HoursPerDay)
            ? Error.New("On duration must be less than 24 hours")
            : new CycleSetting(startTime, onDuration, value);
}
