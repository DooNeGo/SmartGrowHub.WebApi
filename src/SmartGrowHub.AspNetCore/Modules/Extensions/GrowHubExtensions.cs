using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Domain.Model.GrowHub.Programs;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

public static class GrowHubExtensions
{
    public static GrowHubDto ToDto(this GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Model,
            growHub.Plant.Map(ToDto).ValueUnsafe(),
            growHub.Modules.Select(ToDto).ToList());

    private static PlantDto ToDto(this Plant plant) => new(plant.Id, plant.Name, plant.PlantedAt);

    private static GrowHubModuleDto ToDto(this GrowHubModule module) =>
        new(module.Id, module.Program.ToDto(), module.Type.ToDto());
    
    private static ModuleTypeDto ToDto(this ModuleType type) => type switch
    {
        ModuleType.Led => ModuleTypeDto.Led,
        ModuleType.DayLight => ModuleTypeDto.DayLight,
        ModuleType.UvLight => ModuleTypeDto.UvLight,
        ModuleType.Heater => ModuleTypeDto.Heater,
        ModuleType.Humidifier => ModuleTypeDto.Humidifier,
        ModuleType.Fan => ModuleTypeDto.Fan,
        ModuleType.WaterPump => ModuleTypeDto.WaterPump,
        ModuleType.AirFlap => ModuleTypeDto.AirFlap,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    private static ModuleProgramDto ToDto(this ModuleProgram program) =>
        program.Match<ModuleProgramDto>(ToDto, ToDto, ToDto, ToDto);
    
    private static DisabledProgramDto ToDto(this DisabledProgram program) => new();

    private static WeeklyProgramDto ToDto(this WeeklyProgram program) =>
        new(program.Id, program.Entries.Select(ToDto).ToList());
    
    private static DailyProgramDto ToDto(this DailyProgram program) =>
        new(program.Id, program.Entries.Select(ToDto).ToList());
    
    private static ManualProgramDto ToDto(this ManualProgram program) => new(program.Id, program.Quantity.ToDto());

    public static TimedQuantityDto<WeekTimeOnlyDto> ToDto(this TimedQuantity<WeekTimeOnly> interval) =>
        new(interval.Quantity.ToDto(), interval.TimeInterval.ToDto());
    
    public static TimedQuantity<WeekTimeOnly> ToDomain(this TimedQuantityDto<WeekTimeOnlyDto> interval) =>
        new(interval.Quantity.ToDomain(), interval.Interval.ToDomain());
    
    public static TimedQuantityDto<TimeOnly> ToDto(this TimedQuantity<TimeOnlyWrapper> interval) =>
        new(interval.Quantity.ToDto(), interval.TimeInterval.ToDto());
    
    public static TimedQuantity<TimeOnlyWrapper> ToDomain(this TimedQuantityDto<TimeOnly> interval) =>
        new(interval.Quantity.ToDomain(), interval.Interval.ToDomain());
    
    public static TimeIntervalDto<TimeOnly> ToDto(this TimeInterval<TimeOnlyWrapper> interval) =>
        new(interval.Start.ToDto(), interval.End.ToDto());
    
    public static TimeInterval<TimeOnlyWrapper> ToDomain(this TimeIntervalDto<TimeOnly> interval) =>
        new(interval.Start.ToDomain(), interval.End.ToDomain());
    
    public static TimeIntervalDto<WeekTimeOnlyDto> ToDto(this TimeInterval<WeekTimeOnly> interval) =>
        new(interval.Start.ToDto(), interval.End.ToDto());
    
    public static TimeInterval<WeekTimeOnly> ToDomain(this TimeIntervalDto<WeekTimeOnlyDto> interval) =>
        new(interval.Start.ToDomain(), interval.End.ToDomain());
    
    public static TimeOnly ToDto(this TimeOnlyWrapper time) => time.Inner;
    
    public static TimeOnlyWrapper ToDomain(this TimeOnly time) => new(time);
    
    public static WeekTimeOnlyDto ToDto(this WeekTimeOnly time) => new(time.DayOfWeek, time.Time);
    
    public static WeekTimeOnly ToDomain(this WeekTimeOnlyDto time) => new(time.DayOfWeek, time.Time);
    
    public static QuantityDto ToDto(this Quantity value) => new(value.Magnitude, value.Unit.ToDto());

    public static Quantity ToDomain(this QuantityDto quantity) =>
        new(quantity.Magnitude, quantity.Unit.ToMeasurementUnit());
    
    public static string ToDto(this MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Celsius => "\u00b0C",
        MeasurementUnit.Percent => "%",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };

    public static MeasurementUnit ToMeasurementUnit(this string unit) => unit switch
    {
        "\u00b0C" => MeasurementUnit.Celsius,
        "%" => MeasurementUnit.Percent,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}