using System.Collections.Immutable;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Domain.Model.GrowHub.Programs;

namespace SmartGrowHub.Application.UseCases.GrowHubs;

public abstract record SetModuleProgramRequest(Id<GrowHubModule> ModuleId)
{
    public T Match<T>(
        Func<SetDisableProgramRequest, T> mapDisabled,
        Func<SetManualProgramRequest, T> mapManual,
        Func<SetDailyProgramRequest, T> mapDaily,
        Func<SetWeeklyProgramRequest, T> mapWeekly)
        => this switch
        {
            SetDisableProgramRequest request => mapDisabled(request),
            SetManualProgramRequest request => mapManual(request),
            SetDailyProgramRequest request => mapDaily(request),
            SetWeeklyProgramRequest request => mapWeekly(request),
            _ => throw new InvalidOperationException()
        };
}

public sealed record SetDisableProgramRequest(Id<GrowHubModule> ModuleId)
    : SetModuleProgramRequest(ModuleId);

public sealed record SetManualProgramRequest(Id<GrowHubModule> ModuleId, Quantity Quantity)
    : SetModuleProgramRequest(ModuleId);

public sealed record SetDailyProgramRequest(
    Id<GrowHubModule> ModuleId,
    ImmutableList<TimedQuantity<TimeOnlyWrapper>> Entries)
    : SetModuleProgramRequest(ModuleId);

public sealed record SetWeeklyProgramRequest(
    Id<GrowHubModule> ModuleId,
    ImmutableList<TimedQuantity<WeekTimeOnly>> Entries)
    : SetModuleProgramRequest(ModuleId);

public sealed class SetModuleProgramUseCase
{
    private readonly IGrowHubModulesRepository _modulesRepository;

    public SetModuleProgramUseCase(IGrowHubModulesRepository modulesRepository) =>
        _modulesRepository = modulesRepository;

    public IO<Unit> SetModuleProgram(SetModuleProgramRequest request, CancellationToken cancellationToken) =>
        from module in _modulesRepository
            .GetById(request.ModuleId, cancellationToken)
            .ToIOOrFail(Error.New("Module id not found"))
        from program in request.Match(
            mapDisabled: _ => Fin.Succ<ModuleProgram>(DisabledProgram.New()),
            mapManual: manual => ManualProgram.New(manual.Quantity).Cast<ManualProgram, ModuleProgram>(),
            mapDaily: daily => DailyProgram.New(daily.Entries).Cast<DailyProgram, ModuleProgram>(),
            mapWeekly: weekly => WeeklyProgram.New(weekly.Entries).Cast<WeeklyProgram, ModuleProgram>())
            .ToIO()
        let updatedModule = module.SetProgram(program)
        from _ in _modulesRepository.Update(updatedModule, cancellationToken)
        select _;
}