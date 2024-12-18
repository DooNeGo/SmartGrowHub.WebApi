namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public interface ISetting
{
    public T Match<T>(Func<ManualSetting, T> mapManual, Func<CycleSetting, T> mapCycle) =>
        this switch
        {
            ManualSetting manual => mapManual(manual),
            CycleSetting cycle => mapCycle(cycle),
            _ => throw new NotImplementedException()
        };
}
