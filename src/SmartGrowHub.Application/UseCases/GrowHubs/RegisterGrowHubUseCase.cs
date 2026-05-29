using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Domain.Model.GrowHub.Programs;

namespace SmartGrowHub.Application.UseCases.GrowHubs;

public sealed class RegisterGrowHubUseCase
{
    private readonly IGrowHubRepository _repository;

    public RegisterGrowHubUseCase(IGrowHubRepository repository)
    {
        _repository = repository;
    }

    public IO<Unit> RegisterGrowHub(Id<User> userId, NonEmptyString model, CancellationToken cancellationToken)
    {
        var growHub = GrowHub.New(userId, model, model,
        [
            GrowHubModule.New(DisabledProgram.New(), ModuleType.Led),
            GrowHubModule.New(DisabledProgram.New(), ModuleType.DayLight),
            GrowHubModule.New(DisabledProgram.New(), ModuleType.UvLight),
            GrowHubModule.New(DisabledProgram.New(), ModuleType.Heater),
            GrowHubModule.New(DisabledProgram.New(), ModuleType.Humidifier),
            GrowHubModule.New(DisabledProgram.New(), ModuleType.AirFlap)
        ], Option.None);

        return _repository.Add(growHub, cancellationToken);
    }
}