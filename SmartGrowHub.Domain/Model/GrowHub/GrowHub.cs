using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Components;

namespace SmartGrowHub.Domain.Model.GrowHub;

public sealed class GrowHub(
    Id<GrowHub> id,
    NonEmptyString name,
    NonEmptyString model,
    HeaterComponent heaterComponent,
    FanComponent fanComponent,
    DayLightComponent dayLightComponent,
    UvLightComponent uvLightComponent,
    Option<Plant> plant)
    : Entity<GrowHub>(id)
{
    private GrowHub(GrowHub original) : this(
        original.Id, original.Name, original.Model,
        original.HeaterComponent, original.FanComponent,
        original.DayLightComponent, original.UvLightComponent,
        original.Plant)
    { }

    public NonEmptyString Name { get; private init; } = name;

    public NonEmptyString Model { get; private init; } = model;
    
    public HeaterComponent HeaterComponent { get; private init; } = heaterComponent;
    
    public FanComponent FanComponent { get; private init; } = fanComponent;
    
    public DayLightComponent DayLightComponent { get; private init; } = dayLightComponent;
    
    public UvLightComponent UvLightComponent { get; private init; } = uvLightComponent;

    public Option<Plant> Plant { get; private init; } = plant;

    public static GrowHub New(
        NonEmptyString name, NonEmptyString model,
        HeaterComponent heaterComponent, FanComponent fanComponent,
        DayLightComponent dayLightComponent, UvLightComponent uvLightComponent,
        Option<Plant> plant) =>
        new(new Id<GrowHub>(), name, model, heaterComponent, fanComponent, dayLightComponent, uvLightComponent, plant);

    public GrowHub UpdatePlant(Option<Plant> plant) =>
        new(this) { Plant = plant };
}