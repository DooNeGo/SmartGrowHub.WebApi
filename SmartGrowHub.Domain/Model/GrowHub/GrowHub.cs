using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Components;
using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model.GrowHub;

public sealed class GrowHub(
    Id<GrowHub> id,
    NonEmptyString name,
    NonEmptyString model,
    ImmutableArray<IGrowHubComponent> components,
    Option<Plant> plant)
    : Entity<GrowHub>(id)
{
    private GrowHub(GrowHub original) : this(
        original.Id, original.Name, original.Model,
        original.Components, original.Plant)
    { }

    public NonEmptyString Name { get; init; } = name;

    public NonEmptyString Model { get; } = model;

    public ImmutableArray<IGrowHubComponent> Components { get; init; } = components;

    public Option<Plant> Plant { get; init; } = plant;

    public static GrowHub New(NonEmptyString name, NonEmptyString model, ImmutableArray<IGrowHubComponent> components, Option<Plant> plant) =>
        new(new Id<GrowHub>(Ulid.NewUlid()), name, model, components, plant);

    public GrowHub UpdatePlant(Option<Plant> plant) =>
        new(this) { Plant = plant };

    public GrowHub UpdateComponent(IGrowHubComponent component) =>
        new(this) { Components = components.Replace(component, component) };
}