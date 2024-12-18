using LanguageExt.UnsafeValueAccess;
using Microsoft.Extensions.Logging;
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

namespace SmartGrowHub.WebApi.Modules.GrowHubs.Endpoints;

public sealed class GetGrowHubsEndpoint
{
    public static Task<IResult> GetGrowHubs(HttpContext context, ILogger<GetGrowHubsEndpoint> logger, CancellationToken cancellationToken) =>
        Task.FromResult((
            from name in NonEmptyString.From("Home")
            from model in NonEmptyString.From("Smart Grow Hub v2")
            from cycleSetting in CycleSetting.From(DateTime.UtcNow, TimeSpan.FromHours(2), Random.Shared.Next(1, 99))
            let components = new IGrowHubComponent[]
            {
                new HeaterComponent(new Id<HeaterComponent>(Ulid.NewUlid()), new ManualSetting(Random.Shared.Next(1, 99))),
                new LightComponent(new Id<LightComponent>(Ulid.NewUlid()), new ManualSetting(Random.Shared.Next(1, 99)), LightTypes.Seedling),
                new FanComponent(new Id<FanComponent>(Ulid.NewUlid()), cycleSetting)
            }.ToImmutableArray()
            select ToDto(GrowHub.New(name, model, components, None)))
            .Match(
                Succ: growHub => Ok(Result.Success(new GrowHubDto[] { growHub } )),
                Fail: error => HandleError(logger, error)));

    private static GrowHubDto ToDto(GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Model, growHub.Plant.Map(ToDto).ValueUnsafe(), growHub.Components.Select(ToDto));

    private static PlantDto ToDto(Plant plant) => new(plant.Id, plant.Name, plant.PlantedAt);

    private static IGrowHubComponentDto ToDto(IGrowHubComponent component) =>
        component.Match<IGrowHubComponentDto>(
            fan => new FanComponentDto(fan.Id, ToDto(fan.Setting)),
            heater => new HeaterComponentDto(heater.Id, ToDto(heater.Setting)),
            light => new LightComponentDto(light.Id, ToDto(light.Setting), (int)light.LightType));

    private static ISettingDto ToDto(ISetting setting) =>
        setting.Match<ISettingDto>(
            manual => new ManualSettingDto(manual.Value),
            cycle => new CycleSettingDto(cycle.StartTime, cycle.OnDuration, cycle.Value));
}
