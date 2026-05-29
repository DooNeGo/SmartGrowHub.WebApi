using System.Collections.Immutable;
using SmartGrowHub.Application.UseCases.GrowHubs;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules.Endpoints;

public sealed class SetModuleProgramEndpoint
{
    public static ValueTask<IResult> SetModuleProgram(string moduleId, SetModuleProgramRequestDto requestDto,
        SetModuleProgramUseCase useCase, ILogger<SetModuleProgramEndpoint> logger,
        CancellationToken cancellationToken) => (
            from id in Domain.Common.Id<GrowHubModule>.From(moduleId).ToIO()
            from request in ToDomain(id, requestDto).ToIO()
            from _ in useCase.SetModuleProgram(request, cancellationToken)
            select _)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            _ => Ok(Result.Success()),
            error => HandleError(logger, error)));

    private static Fin<SetModuleProgramRequest> ToDomain(Id<GrowHubModule> id, SetModuleProgramRequestDto requestDto) =>
        requestDto.Type switch
        {
            ProgramTypeDto.Disabled => Fin.Succ<SetModuleProgramRequest>(new SetDisableProgramRequest(id)),
            ProgramTypeDto.Manual => ToManual(id, requestDto).Cast<SetManualProgramRequest, SetModuleProgramRequest>(),
            ProgramTypeDto.Daily => ToDaily(id, requestDto).Cast<SetDailyProgramRequest, SetModuleProgramRequest>(),
            ProgramTypeDto.Weekly => ToWeekly(id, requestDto).Cast<SetWeeklyProgramRequest, SetModuleProgramRequest>(),
            _ => throw new InvalidOperationException()
        };

    private static Fin<SetManualProgramRequest> ToManual(Id<GrowHubModule> id, SetModuleProgramRequestDto requestDto) =>
        requestDto.ManualEntry is null
            ? Error.New("Manual entry was null")
            : Fin.Succ(new SetManualProgramRequest(id, requestDto.ManualEntry.ToDomain()));

    private static Fin<SetDailyProgramRequest> ToDaily(Id<GrowHubModule> id, SetModuleProgramRequestDto requestDto) =>
        requestDto.DailyEntries is null
            ? Error.New("Daily entries was null")
            : Fin.Succ(new SetDailyProgramRequest(id,
                requestDto.DailyEntries.Select(x => x.ToDomain()).ToImmutableList()));

    private static Fin<SetWeeklyProgramRequest> ToWeekly(Id<GrowHubModule> id, SetModuleProgramRequestDto requestDto) =>
        requestDto.WeeklyEntries is null
            ? Error.New("Weekly entries was null")
            : Fin.Succ(new SetWeeklyProgramRequest(id,
                requestDto.WeeklyEntries.Select(x => x.ToDomain()).ToImmutableList()));
}