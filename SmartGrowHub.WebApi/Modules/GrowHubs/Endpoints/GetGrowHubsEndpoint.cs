using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Domain.Model.GrowHub.Components;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.Components;
using SmartGrowHub.Shared.Results;
using System.Collections.Immutable;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;
using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.GrowHubs.Endpoints;

public sealed class GetGrowHubsEndpoint
{
    public static Task<IResult> GetGrowHubs(HttpContext context, ILogger<GetGrowHubsEndpoint> logger,
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
                        new HeaterComponent(new Id<GrowHubComponent>(), dailyProgram),
                        new FanComponent(new Id<GrowHubComponent>(), cycleProgram),
                        new DayLightComponent(new Id<GrowHubComponent>(), weeklyProgram),
                        new UvLightComponent(new Id<GrowHubComponent>(), manualProgram)
                    ],
                    None))
        ).Match(
            growHub => Ok(Result.Success(new[] { growHub })),
            error => HandleError(logger, error)));
    }

    private static GrowHubDto ToDto(GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Model,
            growHub.Plant.Map(ToDto).ValueUnsafe(),
            growHub.Components.Select(ToDto));

    private static PlantDto ToDto(Plant plant) => new(plant.Id, plant.Name, plant.PlantedAt);

    private static GrowHubComponentDto ToDto(GrowHubComponent component) =>
        component.Match<GrowHubComponentDto>(ToDto, ToDto, ToDto, ToDto);

    private static HeaterComponentDto ToDto(HeaterComponent component) =>
        new(component.Id, ToDto(component.Program));
    
    private static FanComponentDto ToDto(FanComponent component) =>
        new(component.Id, ToDto(component.Program));

    private static DayLightComponentDto ToDto(DayLightComponent component) =>
        new(component.Id, ToDto(component.Program));

    private static UvLightComponentDto ToDto(UvLightComponent component) =>
        new(component.Id, ToDto(component.Program));

    private static ComponentProgramDto ToDto(ComponentProgram program) =>
        program.Match<ComponentProgramDto>(ToDto, ToDto, ToDto, ToDto);

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