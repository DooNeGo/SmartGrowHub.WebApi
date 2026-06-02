using System.Collections.Immutable;
using SmartGrowHub.Application.UseCases.GrowHubs;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model.Programs;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules.Endpoints;

public sealed class SetModuleProgramEndpoint
{
    public static ValueTask<IResult> SetSchedule(string scheduleId, SetScheduleRequestDto requestDto,
        SetScheduleUseCase useCase, ILogger<SetModuleProgramEndpoint> logger,
        CancellationToken cancellationToken) => (
            from id in Domain.Common.Id<ModuleSchedule>.From(scheduleId).ToIO()
            from request in ToDomain(id, requestDto).ToIO()
            from _ in useCase.SetModuleProgram(request)
            select _)
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            _ => Ok(Result.Success()),
            error => HandleError(logger, error)));

    private static Fin<SetScheduleRequest> ToDomain(Id<ModuleSchedule> id, SetScheduleRequestDto requestDto) =>
        requestDto.Type switch
        {
            ScheduleTypeDto.Disabled => Fin.Succ<SetScheduleRequest>(new SetDisableScheduleRequest(id)),
            ScheduleTypeDto.Enabled => Fin.Succ<SetScheduleRequest>(new SetEnabledScheduleRequest(id)),
            ScheduleTypeDto.Daily => ToDaily(id, requestDto).Cast<SetDailyScheduleRequest, SetScheduleRequest>(),
            ScheduleTypeDto.Weekly => ToWeekly(id, requestDto).Cast<SetWeeklyScheduleRequest, SetScheduleRequest>(),
            _ => throw new InvalidOperationException()
        };

    private static Fin<SetDailyScheduleRequest> ToDaily(Id<ModuleSchedule> id, SetScheduleRequestDto requestDto) =>
        requestDto.DailyEntries is null
            ? Error.New("Daily entries was null")
            : Fin.Succ(new SetDailyScheduleRequest(id,
                requestDto.DailyEntries.Select(x => x.ToDomain()).ToImmutableList()));

    private static Fin<SetWeeklyScheduleRequest> ToWeekly(Id<ModuleSchedule> id, SetScheduleRequestDto requestDto) =>
        requestDto.WeeklyEntries is null
            ? Error.New("Weekly entries was null")
            : Fin.Succ(new SetWeeklyScheduleRequest(id,
                requestDto.WeeklyEntries.Select(x => x.ToDomain()).ToImmutableList()));
}