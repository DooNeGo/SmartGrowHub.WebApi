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

public sealed class UpdateScheduleEndpoint
{
    public static ValueTask<IResult> UpdateSchedule(string scheduleId, UpdateScheduleRequestDto requestDto,
        UpdateScheduleUseCase useCase, ILogger<UpdateScheduleEndpoint> logger,
        CancellationToken cancellationToken) => (
            from id in Domain.Common.Id<ModuleSchedule>.From(scheduleId).ToIO()
            from request in ToDomain(id, requestDto).ToIO()
            from _ in useCase.UpdateSchedule(request)
            select _)
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            _ => Ok(Result.Success()),
            error => HandleError(logger, error)));

    private static Fin<UpdateScheduleRequest> ToDomain(Id<ModuleSchedule> id, UpdateScheduleRequestDto requestDto) =>
        requestDto.Type switch
        {
            ScheduleTypeDto.Disabled => Fin.Succ<UpdateScheduleRequest>(new UpdateDisableScheduleRequest(id)),
            ScheduleTypeDto.Enabled => Fin.Succ<UpdateScheduleRequest>(new UpdateEnabledScheduleRequest(id)),
            ScheduleTypeDto.Daily => ToDaily(id, requestDto).Cast<UpdateDailyScheduleRequest, UpdateScheduleRequest>(),
            ScheduleTypeDto.Weekly => ToWeekly(id, requestDto).Cast<UpdateWeeklyScheduleRequest, UpdateScheduleRequest>(),
            _ => throw new InvalidOperationException()
        };

    private static Fin<UpdateDailyScheduleRequest> ToDaily(Id<ModuleSchedule> id, UpdateScheduleRequestDto requestDto) =>
        requestDto.DailyEntries is null
            ? Error.New("Daily entries was null")
            : Fin.Succ(new UpdateDailyScheduleRequest(id,
                requestDto.DailyEntries.Select(x => x.ToDomain()).ToImmutableList()));

    private static Fin<UpdateWeeklyScheduleRequest> ToWeekly(Id<ModuleSchedule> id, UpdateScheduleRequestDto requestDto) =>
        requestDto.WeeklyEntries is null
            ? Error.New("Weekly entries was null")
            : Fin.Succ(new UpdateWeeklyScheduleRequest(id,
                requestDto.WeeklyEntries.Select(x => x.ToDomain()).ToImmutableList()));
}