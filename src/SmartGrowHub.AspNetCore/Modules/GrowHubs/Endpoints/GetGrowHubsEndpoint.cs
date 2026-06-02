using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Model.Programs;
using SmartGrowHub.Shared.GrowHubs.Model;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Endpoints;

internal sealed class GetGrowHubsEndpoint
{
    public static ValueTask<IResult> GetGrowHubs(IGrowHubRepository growHubRepository, HttpContext context,
        IAccessTokenReader accessTokenReader, ILogger<GetGrowHubsEndpoint> logger,
        CancellationToken cancellationToken) => (
            from userId in accessTokenReader.GetUserId(context)
            from growHubs in growHubRepository.GetAllByUserId(userId)
            select growHubs.Map(ToDto).AsEnumerable())
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            growHub => Ok(Result.Success(growHub)),
            error => HandleError(logger, error)));

    private static GrowHubDto ToDto(GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Model,
            growHub.Plant.Map(ToDto).ValueUnsafe(),
            growHub.Modules.Select(ToDto).ToList());

    private static PlantDto ToDto(Plant plant) => new(plant.Id, plant.Name, plant.PlantedAt);

    private static GrowHubModuleDto ToDto(GrowHubModule module) =>
        new(module.Id, ToDto(module.Schedule), ToDto(module.Type));
    
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

    private static ScheduleDto ToDto(ModuleSchedule schedule) =>
        schedule.Match<ScheduleDto>(ToDto, ToDto, ToDto, ToDto);
    
    private static DisabledScheduleDto ToDto(DisabledSchedule schedule) => new(schedule.Id);
    
    private static EnabledScheduleDto ToDto(EnabledSchedule schedule) => new(schedule.Id);

    private static WeeklyScheduleDto ToDto(WeeklySchedule schedule) =>
        new(schedule.Id, schedule.Entries.Select(x => x.ToDto()).ToList());
    
    private static DailyScheduleDto ToDto(DailySchedule schedule) =>
        new(schedule.Id, schedule.Entries.Select(x => x.ToDto()).ToList());
}