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

    public IO<Unit> RegisterGrowHub(Id<User> userId, NonEmptyString model, CancellationToken cancellationToken)
    {
        var id = new Id<GrowHub>();

        IEnumerable<ModuleType> modulesTypes =
        [
            ModuleType.Led, ModuleType.DayLight, ModuleType.UvLight,
            ModuleType.Heater, ModuleType.Humidifier, ModuleType.AirFlap
        ];
        
        var growHub = new GrowHub(id, userId, model, model,
            modulesTypes.Select(type => CreateDefaultModule(id, type)).ToImmutableList(), Option.None);

        return _repository.Add(growHub, cancellationToken);
    }

    private static GrowHubModule CreateDefaultModule(Id<GrowHub> id, ModuleType type) =>
        GrowHubModule.New(id, DisabledProgram.New(), type);
}