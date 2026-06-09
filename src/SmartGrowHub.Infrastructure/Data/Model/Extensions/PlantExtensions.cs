using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class PlantExtensions
{
    public static PlantDb ToDb(this Plant plant) => new()
    {
        Id = plant.Id,
        GrowHubId = plant.GrowHubId,
        Name = plant.Name,
        PlantedAt = plant.PlantedAt
    };

    public static Fin<Plant> ToDomain(this PlantDb plant) =>
        from name in NonEmptyString.From(plant.Name)
        let id = new Id<Plant>(plant.Id)
        let growHubId = new Id<GrowHub>(plant.GrowHubId)
        select new Plant(id, growHubId, name, plant.PlantedAt);
}