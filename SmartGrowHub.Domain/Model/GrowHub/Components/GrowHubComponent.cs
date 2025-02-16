using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public abstract class GrowHubComponent(Id<GrowHubComponent> id, ComponentProgram program) : Entity<GrowHubComponent>(id)
{
    public ComponentProgram Program { get; } = program;

    public T Match<T>(
        Func<DayLightComponent, T> mapDayLight,
        Func<UvLightComponent, T> mapUvLight,
        Func<HeaterComponent, T> mapHeater,
        Func<FanComponent, T> mapFan) =>
        this switch
        {
            DayLightComponent component => mapDayLight(component),
            UvLightComponent component => mapUvLight(component),
            HeaterComponent component => mapHeater(component),
            FanComponent component => mapFan(component),
            _ => throw new InvalidOperationException()
        };
}
