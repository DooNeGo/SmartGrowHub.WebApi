using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

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