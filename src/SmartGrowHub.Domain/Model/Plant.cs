using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model;

public sealed class Plant(
    Id<Plant> id,
    NonEmptyString name,
    DateTime plantedAt)
    : Entity<Plant>(id)
{
    private Plant(Plant original) : this(
        original.Id, original.Name,
        original.PlantedAt)
    { }

    public NonEmptyString Name { get; init; } = name;

    public DateTime PlantedAt { get; init; } = plantedAt;

    public Plant UpdateName(NonEmptyString name) =>
        new(this) { Name = name };
}