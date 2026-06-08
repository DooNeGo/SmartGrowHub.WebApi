using System.Collections.Immutable;
using System.Numerics;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.UseCases.GrowHubs;

public readonly record struct ScheduleUnitTemplate<T>(
    ScheduleUnitKind Kind,
    Quantity Quantity,
    TimeInterval<T> Interval)
    where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan>;

public abstract record UpdateScheduleRequest(Id<ModuleSchedule> ScheduleId)
{
    public T Match<T>(
        Func<UpdateDisableScheduleRequest, T> mapDisabled,
        Func<UpdateEnabledScheduleRequest, T> mapEnabled,
        Func<UpdateDailyScheduleRequest, T> mapDaily,
        Func<UpdateWeeklyScheduleRequest, T> mapWeekly) =>
        this switch
        {
            UpdateDisableScheduleRequest request => mapDisabled(request),
            UpdateEnabledScheduleRequest request => mapEnabled(request),
            UpdateDailyScheduleRequest request => mapDaily(request),
            UpdateWeeklyScheduleRequest request => mapWeekly(request),
            _ => throw new InvalidOperationException()
        };
}

public sealed record UpdateDisableScheduleRequest(Id<ModuleSchedule> ScheduleId)
    : UpdateScheduleRequest(ScheduleId);

public sealed record UpdateEnabledScheduleRequest(Id<ModuleSchedule> ScheduleId)
    : UpdateScheduleRequest(ScheduleId);

public sealed record UpdateDailyScheduleRequest(
    Id<ModuleSchedule> ScheduleId,
    ImmutableList<ScheduleUnitTemplate<TimeOnlyWrapper>> Entries)
    : UpdateScheduleRequest(ScheduleId);

public sealed record UpdateWeeklyScheduleRequest(
    Id<ModuleSchedule> ScheduleId,
    ImmutableList<ScheduleUnitTemplate<WeekTimeOnly>> Entries)
    : UpdateScheduleRequest(ScheduleId);

public sealed class UpdateScheduleUseCase
{
    private readonly ISchedulesRepository _schedulesRepository;
    private readonly IGrowHubModulesRepository _modulesRepository;
    private readonly IModuleCommandService _moduleCommandService;

    public UpdateScheduleUseCase(
        ISchedulesRepository schedulesRepository,
        IGrowHubModulesRepository modulesRepository,
        IModuleCommandService moduleCommandService)
    {
        _schedulesRepository = schedulesRepository;
        _modulesRepository = modulesRepository;
        _moduleCommandService = moduleCommandService;
    }

    public IO<Unit> UpdateSchedule(UpdateScheduleRequest request) =>
        from module in _modulesRepository
            .GetByScheduleId(request.ScheduleId)
            .ToIOOrFail(Error.New("Schedule id not found"))
        let oldSchedule = module.Schedule
        let scheduleId = oldSchedule.Id
        let moduleId = oldSchedule.ModuleId
        from newSchedule in request
            .Match(
                mapDisabled: _ => Fin.Succ<ModuleSchedule>(new DisabledSchedule(scheduleId, moduleId)),
                mapEnabled: _ => Fin.Succ<ModuleSchedule>(new EnabledSchedule(scheduleId, moduleId)),
                mapDaily: daily => ToDailySchedule(daily, moduleId).Cast<DailySchedule, ModuleSchedule>(),
                mapWeekly: weekly => ToWeeklySchedule(weekly, moduleId).Cast<WeeklySchedule, ModuleSchedule>())
            .As().ToIO()
        from _1 in _schedulesRepository.UpdateAndSave(newSchedule)
        from _2 in _moduleCommandService.ChangeSchedule(module, newSchedule)
        select _2;
    
    private static Fin<DailySchedule> ToDailySchedule(UpdateDailyScheduleRequest request, Id<GrowHubModule> moduleId) =>
        from units in request.Entries.ToScheduleUnits(request.ScheduleId)
        from schedule in DailySchedule.New(units.ToImmutableList(), moduleId, request.ScheduleId)
        select schedule;
    
    private static Fin<WeeklySchedule> ToWeeklySchedule(UpdateWeeklyScheduleRequest request, Id<GrowHubModule> moduleId) =>
        from units in request.Entries.ToScheduleUnits(request.ScheduleId)
        from schedule in WeeklySchedule.New(units.ToImmutableList(), moduleId, request.ScheduleId)
        select schedule;
}

public static class ScheduleUnitTemplateExtensions
{
    public static Fin<Iterable<ScheduleUnit<T>>> ToScheduleUnits<T>(this IEnumerable<ScheduleUnitTemplate<T>> templates,
        Id<ModuleSchedule> scheduleId)
        where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan> =>
        templates.AsIterable().Traverse(template => template.ToScheduleUnit(scheduleId)).As();

    public static Fin<ScheduleUnit<T>> ToScheduleUnit<T>(this ScheduleUnitTemplate<T> template,
        Id<ModuleSchedule> scheduleId)
        where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan> =>
        ScheduleUnit<T>.New(scheduleId, template.Kind, template.Quantity, template.Interval);
}