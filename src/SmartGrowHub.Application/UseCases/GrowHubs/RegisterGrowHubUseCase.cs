using System.Collections.Immutable;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.UseCases.GrowHubs;

public sealed class RegisterGrowHubUseCase
{
    private readonly IGrowHubRepository _repository;

    public RegisterGrowHubUseCase(IGrowHubRepository repository) => _repository = repository;

    public IO<Unit> RegisterGrowHub(Id<User> userId, NonEmptyString model)
    {
        var id = new Id<GrowHub>();

        IEnumerable<ModuleType> modulesTypes =
        [
            ModuleType.Led, ModuleType.DayLight, ModuleType.UvLight,
            ModuleType.Heater, ModuleType.Humidifier, ModuleType.AirFlap
        ];
        
        var growHub = new GrowHub(id, userId, model, model,
            modulesTypes.Select(type => CreateDefaultModule(id, type)).ToImmutableList(), Option.None);

        return _repository.AddAndSave(growHub);
    }

    private static GrowHubModule CreateDefaultModule(Id<GrowHub> growHubId, ModuleType type)
    {
        var id = new Id<GrowHubModule>();
        return new GrowHubModule(id, growHubId, DisabledSchedule.New(id), type);
    }
}