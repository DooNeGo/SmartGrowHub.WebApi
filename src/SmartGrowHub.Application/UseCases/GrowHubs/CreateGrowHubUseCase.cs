using System.Collections.Immutable;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.UseCases.GrowHubs;

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
        
        return
            from id in Domain.Common.Id<GrowHub>.From("01KSZJND2Z4K7CMQJ7B1NVG2BW").ToIO()
            let growHub = new GrowHub(id, userId, model, model,
                modulesTypes.Select(type => CreateDefaultModule(id, type)).ToImmutableList(), Option.None)
            from _ in _repository.AddAndSave(growHub)
            select _;
    }

    private static GrowHubModule CreateDefaultModule(Id<GrowHub> growHubId, ModuleType type)
    {
        var id = new Id<GrowHubModule>();
        return new GrowHubModule(id, growHubId, DisabledSchedule.New(id), type);
    }
}