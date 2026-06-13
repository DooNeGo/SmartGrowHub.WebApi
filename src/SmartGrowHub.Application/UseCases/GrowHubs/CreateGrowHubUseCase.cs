using System.Collections.Immutable;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.UseCases.GrowHubs;

public static class GrowHubsDefaults
{
    public static readonly Id<GrowHub> GrowHubId =
        Domain.Common.Id<GrowHub>.From("01KSZJND2Z4K7CMQJ7B1NVG2BW").ThrowIfFail();
}

public sealed class CreateGrowHubUseCase
{
    private readonly IGrowHubRepository _repository;

    public CreateGrowHubUseCase(IGrowHubRepository repository) => _repository = repository;

    public IO<Unit> CreateGrowHub(Id<User> userId, NonEmptyString model)
    {
        IEnumerable<ModuleType> modulesTypes =
        [
            ModuleType.Led, ModuleType.DayLight, ModuleType.UvLight,
            ModuleType.Heater, ModuleType.Humidifier, ModuleType.AirFlap,
            ModuleType.Fan, ModuleType.WaterPump
        ];

        Id<GrowHub> id = GrowHubsDefaults.GrowHubId;
        
        ImmutableList<GrowHubModule> modules = modulesTypes
            .Select(type => CreateDefaultModule(id, type))
            .ToImmutableList();
        
        GrowHub growHub = new(id, userId, model, model, modules, Option.None);

        return _repository.Add(growHub);
    }

    private static GrowHubModule CreateDefaultModule(Id<GrowHub> growHubId, ModuleType type)
    {
        var id = new Id<GrowHubModule>();
        return new GrowHubModule(id, growHubId, DisabledSchedule.New(id), type);
    }
}