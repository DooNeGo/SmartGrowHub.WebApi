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
using SmartGrowHub.Domain.Model.GrowHub.Schedules;
using SmartGrowHub.Shared.GrowHubs.Schedules;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.GrowHubs.Endpoints;

public sealed class GetGrowHubsEndpoint
{
    public static Task<IResult> GetGrowHubs(HttpContext context, ILogger<GetGrowHubsEndpoint> logger,
        CancellationToken cancellationToken) =>
        Task.FromResult((
            from name in NonEmptyString.From("Home")
            from model in NonEmptyString.From("Smart Grow Hub v2")
            from cycleInterval in ValueWithInterval<TimeOnlyWrapper>.New(
                new SettingValue(10, MeasurementUnit.Percent),
                new TimeInterval<TimeOnlyWrapper>(
                    new TimeOnlyWrapper(new TimeOnly(20, 0)),
                    new TimeOnlyWrapper(new TimeOnly(9, 0))))
            from dayIntervals in ImmutableArray.Create(
                ValueWithInterval<TimeOnlyWrapper>.New(
                    new SettingValue(10, MeasurementUnit.Percent),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(8, 0)),
                        new TimeOnlyWrapper(new TimeOnly(12, 0)))),
                ValueWithInterval<TimeOnlyWrapper>.New(
                    new SettingValue(20, MeasurementUnit.Celsius),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(12, 0)),
                        new TimeOnlyWrapper(new TimeOnly(16, 0)))),
                ValueWithInterval<TimeOnlyWrapper>.New(
                    new SettingValue(30, MeasurementUnit.Percent),
                    new TimeInterval<TimeOnlyWrapper>(
                        new TimeOnlyWrapper(new TimeOnly(23, 0)),
                        new TimeOnlyWrapper(new TimeOnly(8, 0))))
            ).AsIterable().Traverse(x => x)
            from weekIntervals in ImmutableArray.Create(
                ValueWithInterval<WeekTimeOnly>.New(
                    new SettingValue(15, MeasurementUnit.Percent),
                    new TimeInterval<WeekTimeOnly>(
                        new WeekTimeOnly(DayOfWeek.Monday, new TimeOnly(8, 0)),
                        new WeekTimeOnly(DayOfWeek.Tuesday, new TimeOnly(12, 0)))),
                ValueWithInterval<WeekTimeOnly>.New(
                    new SettingValue(26.8f, MeasurementUnit.Celsius),
                    new TimeInterval<WeekTimeOnly>(
                        new WeekTimeOnly(DayOfWeek.Tuesday, new TimeOnly(12, 0)),
                        new WeekTimeOnly(DayOfWeek.Thursday, new TimeOnly(19, 0)))),
                ValueWithInterval<WeekTimeOnly>.New(
                    new SettingValue(10, MeasurementUnit.Percent),
                    new TimeInterval<WeekTimeOnly>(
                        new WeekTimeOnly(DayOfWeek.Thursday, new TimeOnly(19, 0)),
                        new WeekTimeOnly(DayOfWeek.Sunday, new TimeOnly(9, 0))))
            ).AsIterable().Traverse(x => x)
            from dailySchedule in DailySchedule.New([..dayIntervals])
            from weeklySchedule in WeeklySchedule.New([..weekIntervals])
            let cycleSetting = CycleSetting.New(cycleInterval)
            let dailySetting = DailySetting.New(dailySchedule)
            let weeklySetting = WeeklySetting.New(weeklySchedule)
            let manualSetting = ManualSetting.New(new SettingValue(100, MeasurementUnit.Percent))
            select ToDto(
                GrowHub.New(name, model,
                    new HeaterComponent(new Id<HeaterComponent>(Ulid.NewUlid()), cycleSetting),
                    new FanComponent(new Id<FanComponent>(Ulid.NewUlid()), dailySetting),
                    new DayLightComponent(new Id<DayLightComponent>(Ulid.NewUlid()), weeklySetting),
                    new UvLightComponent(new Id<UvLightComponent>(Ulid.NewUlid()), manualSetting),
                    None))
        ).Match(
            Succ: growHub => Ok(Result.Success(new[] { growHub })),
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

    private static WeeklySettingDto ToDto(WeeklySetting setting) =>
        new(setting.Id, ToDto(setting.Schedule));
    
    private static DailySettingDto ToDto(DailySetting setting) =>
        new(setting.Id, ToDto(setting.Schedule));
    
    private static WeeklyScheduleDto ToDto(WeeklySchedule schedule) =>
        new(schedule.Id, [..schedule.Intervals.Select(ToDto)]);
    
    private static DailyScheduleDto ToDto(DailySchedule schedule) =>
        new(schedule.Id, [..schedule.Intervals.Select(ToDto)]);
    
    private static CycleSettingDto ToDto(CycleSetting setting) =>
        new(setting.Id, ToDto(setting.Interval));
    
    private static ManualSettingDto ToDto(ManualSetting setting) => new(setting.Id, ToDto(setting.Value));

    private static ValueWithIntervalDto<WeekTimeOnlyDto> ToDto(ValueWithInterval<WeekTimeOnly> interval) =>
        new(interval.Id, ToDto(interval.Value), ToDto(interval.TimeInterval));
    
    private static ValueWithIntervalDto<TimeOnly> ToDto(ValueWithInterval<TimeOnlyWrapper> interval) =>
        new(interval.Id, ToDto(interval.Value), ToDto(interval.TimeInterval));
    
    private static TimeIntervalDto<TimeOnly> ToDto(TimeInterval<TimeOnlyWrapper> interval) =>
        new(ToDto(interval.Start), ToDto(interval.End));
    
    private static TimeIntervalDto<WeekTimeOnlyDto> ToDto(TimeInterval<WeekTimeOnly> interval) =>
        new(ToDto(interval.Start), ToDto(interval.End));
    
    private static TimeOnly ToDto(TimeOnlyWrapper time) => time.Inner;
    
    private static WeekTimeOnlyDto ToDto(WeekTimeOnly time) => new(time.DayOfWeek, time.TimeOnly);
    
    private static SettingValueDto ToDto(SettingValue value) => new(value.Magnitude, ToDto(value.Unit));

    private static string ToDto(MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Celsius => "\u00b0C",
        MeasurementUnit.Percent => "%",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}