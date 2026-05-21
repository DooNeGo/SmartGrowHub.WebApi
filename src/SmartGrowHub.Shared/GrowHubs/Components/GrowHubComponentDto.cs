using System.Text.Json.Serialization;
using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Shared.GrowHubs.Components;

[JsonDerivedType(typeof(DayLightComponentDto), "DayLight")]
[JsonDerivedType(typeof(UvLightComponentDto), "UvLight")]
[JsonDerivedType(typeof(HeaterComponentDto), "Heater")]
[JsonDerivedType(typeof(FanComponentDto), "Fan")]
public abstract record GrowHubComponentDto(Ulid Id, ComponentProgramDto Program)
{
    public T Match<T>(
        Func<DayLightComponentDto, T> mapDayLight,
        Func<UvLightComponentDto, T> mapUvLight,
        Func<HeaterComponentDto, T> mapHeater,
        Func<FanComponentDto, T> mapFan) =>
        this switch
        {
            DayLightComponentDto dayLight => mapDayLight(dayLight),
            UvLightComponentDto uvLight => mapUvLight(uvLight),
            HeaterComponentDto heater => mapHeater(heater),
            FanComponentDto fan => mapFan(fan),
            _ => throw new InvalidOperationException()
        };
}