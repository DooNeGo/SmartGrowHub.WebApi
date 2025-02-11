using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Domain.Model.GrowHub.Components;
using SmartGrowHub.Domain.Model.GrowHub.Settings;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.Components;
using SmartGrowHub.Shared.GrowHubs.Settings;
using SmartGrowHub.Shared.Results;
using System.Collections.Immutable;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;
using Unit = SmartGrowHub.Domain.Model.GrowHub.Settings.Unit;

namespace SmartGrowHub.WebApi.Modules.GrowHubs.Endpoints;

public sealed class GetGrowHubsEndpoint
{
    public static Task<IResult> GetGrowHubs(HttpContext context, ILogger<GetGrowHubsEndpoint> logger,
        CancellationToken cancellationToken) =>
        Task.FromResult((
            from name in NonEmptyString.From("Home")
            from model in NonEmptyString.From("Smart Grow Hub v2")
            from cycleSetting in CycleSetting<TimeOnlyWrapper>.New(
                new SettingValue(55, Unit.Percent),
                new TimePeriod<TimeOnlyWrapper>(
                    new TimeOnlyWrapper(new TimeOnly(8, 0)),
                    new TimeOnlyWrapper(new TimeOnly(20, 0))))
            from dayCycles in ImmutableArray
                .Create(
                [
                    CycleSetting<TimeOnlyWrapper>.New(
                        new SettingValue(29.7f, Unit.Celsius),
                        new TimePeriod<TimeOnlyWrapper>(
                            new TimeOnlyWrapper(new TimeOnly(8, 0)),
                            new TimeOnlyWrapper(new TimeOnly(10, 10)))),
                    
                    CycleSetting<TimeOnlyWrapper>.New(
                        new SettingValue(31.2f, Unit.Celsius),
                        new TimePeriod<TimeOnlyWrapper>(
                            new TimeOnlyWrapper(new TimeOnly(10, 11)),
                            new TimeOnlyWrapper(new TimeOnly(12, 30)))),
                    
                    CycleSetting<TimeOnlyWrapper>.New(
                        new SettingValue(90, Unit.Percent),
                        new TimePeriod<TimeOnlyWrapper>(
                            new TimeOnlyWrapper(new TimeOnly(13, 0)),
                            new TimeOnlyWrapper(new TimeOnly(20, 0)))),
                ])
                .AsIterable()
                .Traverse(s => s)
            from daySchedule in DayScheduleSetting.New([.. dayCycles])
            from weekCycles in ImmutableArray
                .Create(
                [
                    CycleSetting<WeekTimeOnly>.New(
                        new SettingValue(10, Unit.Percent),
                        new TimePeriod<WeekTimeOnly>(
                            new WeekTimeOnly(DayOfWeek.Monday, new TimeOnly(5, 0)),
                            new WeekTimeOnly(DayOfWeek.Tuesday, new TimeOnly(10, 0)))),
                    
                    CycleSetting<WeekTimeOnly>.New(
                        new SettingValue(55, Unit.Percent),
                        new TimePeriod<WeekTimeOnly>(
                            new WeekTimeOnly(DayOfWeek.Tuesday, new TimeOnly(11, 0)),
                            new WeekTimeOnly(DayOfWeek.Wednesday, new TimeOnly(10, 0)))),
                    
                    CycleSetting<WeekTimeOnly>.New(
                        new SettingValue(41.2f, Unit.Celsius),
                        new TimePeriod<WeekTimeOnly>(
                            new WeekTimeOnly(DayOfWeek.Wednesday, new TimeOnly(19, 10)),
                            new WeekTimeOnly(DayOfWeek.Friday, new TimeOnly(10, 0)))),
                ])
                .AsIterable()
                .Traverse(s => s)
            from weekSchedule in WeekScheduleSetting.New([.. weekCycles])
            select ToDto(GrowHub.New(name, model,
                new HeaterComponent(new Id<HeaterComponent>(Ulid.NewUlid()), cycleSetting),
                new FanComponent(new Id<FanComponent>(Ulid.NewUlid()), daySchedule),
                new DayLightComponent(new Id<DayLightComponent>(Ulid.NewUlid()), weekSchedule),
                new UvLightComponent(new Id<UvLightComponent>(Ulid.NewUlid()), daySchedule),
                None)))
            .Match(
                Succ: growHub => Ok(Result.Success(new[] { growHub } )),
                Fail: error => HandleError(logger, error)));

    private static GrowHubDto ToDto(GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Model,
            growHub.Plant.Map(ToDto).ValueUnsafe(), ToDto(growHub.HeaterComponent),
            ToDto(growHub.FanComponent), ToDto(growHub.DayLightComponent),
            ToDto(growHub.UvLightComponent));

    private static PlantDto ToDto(Plant plant) => new(plant.Id, plant.Name, plant.PlantedAt);

    private static HeaterComponentDto ToDto(HeaterComponent component) =>
        new(component.Id, ToDto(component.Setting));
    
    private static FanComponentDto ToDto(FanComponent component) =>
        new(component.Id, ToDto(component.Setting));

    private static DayLightComponentDto ToDto(DayLightComponent component) =>
        new(component.Id, ToDto(component.Setting));

    private static UvLightComponentDto ToDto(UvLightComponent component) =>
        new(component.Id, ToDto(component.Setting));

    private static SettingDto ToDto(Setting setting) =>
        setting.Match<SettingDto>(ToDto, ToDto, ToDto, ToDto);

    private static WeekScheduleSettingDto ToDto(WeekScheduleSetting setting) =>
        new([..setting.Schedules.Select(ToDto)]);
    
    private static DayScheduleSettingDto ToDto(DayScheduleSetting setting) =>
        new([..setting.Schedules.Select(ToDto)]);
    
    private static CycleSettingDto<TimeOnly> ToDto(CycleSetting<TimeOnlyWrapper> setting) =>
        new(ToDto(setting.Value), ToDto(setting.TimePeriod));
    
    private static CycleSettingDto<WeekTimeOnlyDto> ToDto(CycleSetting<WeekTimeOnly> setting) =>
        new(ToDto(setting.Value), ToDto(setting.TimePeriod));
    
    private static ManualSettingDto ToDto(ManualSetting setting) => new(ToDto(setting.Value));
    
    private static TimePeriodDto<TimeOnly> ToDto(TimePeriod<TimeOnlyWrapper> period) =>
        new(ToDto(period.Start), ToDto(period.End));
    
    private static TimePeriodDto<WeekTimeOnlyDto> ToDto(TimePeriod<WeekTimeOnly> period) =>
        new(ToDto(period.Start), ToDto(period.End));
    
    private static TimeOnly ToDto(TimeOnlyWrapper time) => time.InnerTimeOnly;
    
    private static WeekTimeOnlyDto ToDto(WeekTimeOnly time) => new(time.DayOfWeek, time.TimeOnly);
    
    private static SettingValueDto ToDto(SettingValue value) => new(value.Value, ToDto(value.Unit));

    private static UnitDto ToDto(Unit unit) => unit switch
    {
        Unit.Celsius => UnitDto.Celsius,
        Unit.Percent => UnitDto.Percent,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}