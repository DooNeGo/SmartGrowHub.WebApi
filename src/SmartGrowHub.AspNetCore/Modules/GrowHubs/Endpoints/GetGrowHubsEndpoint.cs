using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.Results;
using System.Collections.Immutable;
using SmartGrowHub.Domain.Model.GrowHub.Programs;
using SmartGrowHub.Shared.GrowHubs.ModulePrograms;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Endpoints;

public sealed class GetGrowHubsEndpoint
{
    public static Task<IResult> GetGrowHubs(ILogger<GetGrowHubsEndpoint> logger,
        CancellationToken cancellationToken)
    {
        return Task.FromResult((
            from name in NonEmptyString.From("Home")
            from model in NonEmptyString.From("Smart Grow Hub v2")
            from cycleInterval in TimedQuantity<TimeOnlyWrapper>.New(
                new Quantity(10, MeasurementUnit.Percent),
                new TimeInterval<TimeOnlyWrapper>(
                    new TimeOnlyWrapper(new TimeOnly(20, 0)),
                    new TimeOnlyWrapper(new TimeOnly(15, 0))))
            from dayIntervals in ImmutableArray.Create(
                TimedQuantity<TimeOnlyWrapper>.New(
                    new Quantity(10, MeasurementUnit.Percent),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(8, 0)),
                        new TimeOnlyWrapper(new TimeOnly(12, 0)))),
                TimedQuantity<TimeOnlyWrapper>.New(
                    new Quantity(20, MeasurementUnit.Celsius),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(14, 0)),
                        new TimeOnlyWrapper(new TimeOnly(16, 0)))),
                TimedQuantity<TimeOnlyWrapper>.New(
                    new Quantity(30, MeasurementUnit.Percent),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(18, 0)),
                        new TimeOnlyWrapper(new TimeOnly(20, 0)))),
                TimedQuantity<TimeOnlyWrapper>.New(
                    new Quantity(27.3f, MeasurementUnit.Celsius),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(21, 0)),
                        new TimeOnlyWrapper(new TimeOnly(23, 59))))
            ).AsIterable().Traverse(x => x)
            from weekIntervals in ImmutableArray.Create(
                TimedQuantity<WeekTimeOnly>.New(
                    new Quantity(15, MeasurementUnit.Percent),
                    new TimeInterval<WeekTimeOnly>(
                        new WeekTimeOnly(DayOfWeek.Monday, new TimeOnly(8, 0)),
                        new WeekTimeOnly(DayOfWeek.Tuesday, new TimeOnly(19, 0)))),
                TimedQuantity<WeekTimeOnly>.New(
                    new Quantity(26.8f, MeasurementUnit.Celsius),
                    new TimeInterval<WeekTimeOnly>(
                        new WeekTimeOnly(DayOfWeek.Wednesday, new TimeOnly(12, 0)),
                        new WeekTimeOnly(DayOfWeek.Thursday, new TimeOnly(19, 0)))),
                TimedQuantity<WeekTimeOnly>.New(
                    new Quantity(10, MeasurementUnit.Percent),
                    new TimeInterval<WeekTimeOnly>(
                        new WeekTimeOnly(DayOfWeek.Friday, new TimeOnly(19, 0)),
                        new WeekTimeOnly(DayOfWeek.Sunday, new TimeOnly(14, 0))))
            ).AsIterable().Traverse(x => x)
            let cycleProgram = CycleProgram.New(cycleInterval)
            from dailyProgram in DailyProgram.New([..dayIntervals])
            from weeklyProgram in WeeklyProgram.New([..weekIntervals])
            let manualProgram = ManualProgram.New(new Quantity(100, MeasurementUnit.Percent))
            select ToDto(
                GrowHub.New(name, model,
                    [
                        new GrowHubModule(new Id<GrowHubModule>(), dailyProgram, ModuleType.Heater),
                        new GrowHubModule(new Id<GrowHubModule>(), cycleProgram, ModuleType.Fan),
                        new GrowHubModule(new Id<GrowHubModule>(), weeklyProgram, ModuleType.DayLight),
                        new GrowHubModule(new Id<GrowHubModule>(), manualProgram, ModuleType.UvLight)
                    ],
                    None))
        ).Match(
            growHub => Ok(Result.Success(new[] { growHub })),
            error => HandleError(logger, error)));
    }

    private static GrowHubDto ToDto(GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Model,
            growHub.Plant.Map(ToDto).ValueUnsafe(),
            growHub.Modules.Select(ToDto));

    private static PlantDto ToDto(Plant plant) => new(plant.Id, plant.Name, plant.PlantedAt);

    private static GrowHubModuleDto ToDto(GrowHubModule module) =>
        new(module.Id, ToDto(module.Program), ToDto(module.Type));
    
    private static ModuleTypeDto ToDto(ModuleType type) => type switch
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

    private static ModuleProgramDto ToDto(ModuleProgram program) =>
        program.Match<ModuleProgramDto>(ToDto, ToDto, ToDto, ToDto);

    private static WeeklyProgramDto ToDto(WeeklyProgram program) =>
        new(program.Id, program.Entries.Select(ToDto));
    
    private static DailyProgramDto ToDto(DailyProgram program) =>
        new(program.Id, program.Entries.Select(ToDto));
    
    private static CycleProgramDto ToDto(CycleProgram program) =>
        new(program.Id, ToDto(program.CycleParameters));
    
    private static ManualProgramDto ToDto(ManualProgram program) => new(program.Id, ToDto(program.Quantity));

    private static TimedQuantityDto<WeekTimeOnlyDto> ToDto(TimedQuantity<WeekTimeOnly> interval) =>
        new(interval.Id, ToDto(interval.Quantity), ToDto(interval.TimeInterval));
    
    private static TimedQuantityDto<TimeOnly> ToDto(TimedQuantity<TimeOnlyWrapper> interval) =>
        new(interval.Id, ToDto(interval.Quantity), ToDto(interval.TimeInterval));
    
    private static TimeIntervalDto<TimeOnly> ToDto(TimeInterval<TimeOnlyWrapper> interval) =>
        new(ToDto(interval.Start), ToDto(interval.End));
    
    private static TimeIntervalDto<WeekTimeOnlyDto> ToDto(TimeInterval<WeekTimeOnly> interval) =>
        new(ToDto(interval.Start), ToDto(interval.End));
    
    private static TimeOnly ToDto(TimeOnlyWrapper time) => time.Inner;
    
    private static WeekTimeOnlyDto ToDto(WeekTimeOnly time) => new(time.DayOfWeek, time.TimeOnly);
    
    private static QuantityDto ToDto(Quantity value) => new(value.Magnitude, ToDto(value.Unit));

    private static string ToDto(MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Celsius => "\u00b0C",
        MeasurementUnit.Percent => "%",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}