namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public interface IGrowHubComponent
{
    public T Match<T>(Func<FanComponent, T> mapFan, Func<HeaterComponent, T> mapHeater, Func<LightComponent, T> mapLight) =>
        this switch
        {
            FanComponent fan => mapFan(fan),
            HeaterComponent heater => mapHeater(heater),
            LightComponent light => mapLight(light),
            _ => throw new NotImplementedException()
        };
}
