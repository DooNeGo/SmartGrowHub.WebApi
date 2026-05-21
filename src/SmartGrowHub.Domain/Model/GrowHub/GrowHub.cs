using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Components;
using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model.GrowHub;

public sealed class GrowHub(
    Id<GrowHub> id,
    NonEmptyString name,
    NonEmptyString model,
    ImmutableArray<GrowHubComponent> components,
    Option<Plant> plant)
    : Entity<GrowHub>(id)
{
    private GrowHub(GrowHub original) : this(
        original.Id, original.Name,
        original.Model, original.Components,
        original.Plant)
    { }

    public NonEmptyString Name { get; private init; } = name;

    public NonEmptyString Model { get; private init; } = model;

    public ImmutableArray<GrowHubComponent> Components { get; private init; } = components;

    public Option<Plant> Plant { get; private init; } = plant;

    public static GrowHub New(NonEmptyString name, NonEmptyString model,
        ImmutableArray<GrowHubComponent> components, Option<Plant> plant) =>
        new(new Id<GrowHub>(), name, model, components, plant);

    public GrowHub UpdatePlant(Option<Plant> plant) =>
        new(this) { Plant = plant };
}