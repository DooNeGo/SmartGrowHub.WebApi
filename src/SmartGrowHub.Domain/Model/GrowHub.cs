using System.Collections.Immutable;
using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model;

public sealed class GrowHub(
    Id<GrowHub> id,
    Id<User> userId,
    NonEmptyString name,
    NonEmptyString model,
    ImmutableList<GrowHubModule> modules,
    Option<Plant> plant)
    : Entity<GrowHub>(id)
{
    private GrowHub(GrowHub original) : this(
        original.Id, original.UserId, original.Name,
        original.Model, original.Modules, original.Plant)
    { }

    public Id<User> UserId { get; private init; } = userId;
    
    public NonEmptyString Name { get; private init; } = name;

    public NonEmptyString Model { get; private init; } = model;

    public ImmutableList<GrowHubModule> Modules { get; private init; } = modules;

    public Option<Plant> Plant { get; private init; } = plant;

    public static GrowHub New(Id<User> userId, NonEmptyString name, NonEmptyString model,
        ImmutableList<GrowHubModule> modules, Option<Plant> plant) =>
        new(new Id<GrowHub>(), userId, name, model, modules, plant);

    public GrowHub UpdatePlant(Option<Plant> plant) =>
        new(this) { Plant = plant };
}