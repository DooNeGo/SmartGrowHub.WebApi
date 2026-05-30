using System.Collections.Immutable;
using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class GrowHubExtensions
{
    public static GrowHubDb ToDb(this GrowHub growHub) => new()
    {
        Id = growHub.Id,
        UserId = growHub.UserId,
        Name = growHub.Name,
        Model = growHub.Model,
        Plant = growHub.Plant.ValueUnsafe()?.ToDb(),
        Modules = growHub.Modules.Select(x => x.ToDb()).ToList()
    };
    
    public static Fin<GrowHub> ToDomain(this GrowHubDb growHub) =>
        from id in Id<GrowHub>.From(growHub.Id)
        from userId in Id<User>.From(growHub.UserId)
        from name in NonEmptyString.From(growHub.Name)
        from model in NonEmptyString.From(growHub.Model)
        from modules in growHub.Modules.AsIterable().Traverse(x => x.ToDomain())
        from plant in Prelude.Optional(growHub.Plant).Traverse(x => x.ToDomain())
        select new GrowHub(id, userId, name, model, modules.ToImmutableList(), plant);
}

internal static class GrowHubModuleExtensions
{
    public static GrowHubModuleDb ToDb(this GrowHubModule module) => new()
    {
        Id = module.Id,
        Type = module.Type.ToDb(),
        Program = module.Program.ToDb(),
        GrowHubId = module.GrowHubId
    };
    
    public static Fin<GrowHubModule> ToDomain(this GrowHubModuleDb module) =>
        from id in Id<GrowHubModule>.From(module.Id)
        from growHubId in Id<GrowHub>.From(module.GrowHubId)
        from program in module.Program.ToDomain()
        let type = module.Type.ToDomain()
        select new GrowHubModule(id, growHubId, program, type);
}

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
        from id in Id<Plant>.From(plant.Id)
        from growHubId in Id<GrowHub>.From(plant.GrowHubId)
        from name in NonEmptyString.From(plant.Name)
        select new Plant(id, growHubId, name, plant.PlantedAt);
}

internal static class ModuleTypeExtensions
{
    public static ModuleTypeDb ToDb(this ModuleType type) => type switch
    {
        ModuleType.Led => ModuleTypeDb.Led,
        ModuleType.DayLight => ModuleTypeDb.DayLight,
        ModuleType.UvLight => ModuleTypeDb.UvLight,
        ModuleType.Heater => ModuleTypeDb.Heater,
        ModuleType.Humidifier => ModuleTypeDb.Humidifier,
        ModuleType.Fan => ModuleTypeDb.Fan,
        ModuleType.WaterPump => ModuleTypeDb.WaterPump,
        ModuleType.AirFlap => ModuleTypeDb.AirFlap,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
    
    public static ModuleType ToDomain(this ModuleTypeDb type) => type switch
    {
        ModuleTypeDb.Led => ModuleType.Led,
        ModuleTypeDb.DayLight => ModuleType.DayLight,
        ModuleTypeDb.UvLight => ModuleType.UvLight,
        ModuleTypeDb.Heater => ModuleType.Heater,
        ModuleTypeDb.Humidifier => ModuleType.Humidifier,
        ModuleTypeDb.Fan => ModuleType.Fan,
        ModuleTypeDb.WaterPump => ModuleType.WaterPump,
        ModuleTypeDb.AirFlap => ModuleType.AirFlap,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}