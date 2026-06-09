using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
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

public abstract record SetScheduleRequest(Id<ModuleSchedule> ScheduleId)
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public T Match<T>(
        Func<SetDisabledScheduleRequest, T> Disabled,
        Func<SetEnabledScheduleRequest, T> Enabled,
        Func<SetDailyScheduleRequest, T> Daily,
        Func<SetWeeklyScheduleRequest, T> Weekly) =>
        this switch
        {
            SetDisabledScheduleRequest request => Disabled(request),
            SetEnabledScheduleRequest request => Enabled(request),
            SetDailyScheduleRequest request => Daily(request),
            SetWeeklyScheduleRequest request => Weekly(request),
            _ => throw new InvalidOperationException()
        };
}

public sealed record SetDisabledScheduleRequest(Id<ModuleSchedule> ScheduleId)
    : SetScheduleRequest(ScheduleId);

public sealed record SetEnabledScheduleRequest(Id<ModuleSchedule> ScheduleId)
    : SetScheduleRequest(ScheduleId);

public sealed record SetDailyScheduleRequest(
    Id<ModuleSchedule> ScheduleId,
    ImmutableList<ScheduleUnitTemplate<TimeOnlyWrapper>> Entries)
    : SetScheduleRequest(ScheduleId);

public sealed record SetWeeklyScheduleRequest(
    Id<ModuleSchedule> ScheduleId,
    ImmutableList<ScheduleUnitTemplate<WeekTimeOnly>> Entries)
    : SetScheduleRequest(ScheduleId);

public sealed class SetScheduleUseCase
{
    private readonly ISchedulesRepository _schedulesRepository;
    private readonly IGrowHubModulesRepository _modulesRepository;
    private readonly IModuleCommandService _moduleCommandService;

    public SetScheduleUseCase(
        ISchedulesRepository schedulesRepository,
        IGrowHubModulesRepository modulesRepository,
        IModuleCommandService moduleCommandService)
    {
        _schedulesRepository = schedulesRepository;
        _modulesRepository = modulesRepository;
        _moduleCommandService = moduleCommandService;
    }

    public IO<Unit> SetSchedule(SetScheduleRequest request) =>
        from module in _modulesRepository
            .GetByScheduleId(request.ScheduleId)
            .ToIOOrFail(Error.New("Schedule id not found"))
        let oldSchedule = module.Schedule
        let scheduleId = oldSchedule.Id
        let moduleId = oldSchedule.ModuleId
        from newSchedule in request
            .Match(
                Disabled: _ => Fin.Succ<ModuleSchedule>(new DisabledSchedule(scheduleId, moduleId)),
                Enabled: _ => Fin.Succ<ModuleSchedule>(new EnabledSchedule(scheduleId, moduleId)),
                Daily: daily => ToDailySchedule(daily, moduleId).Cast<DailySchedule, ModuleSchedule>(),
                Weekly: weekly => ToWeeklySchedule(weekly, moduleId).Cast<WeeklySchedule, ModuleSchedule>())
            .As().ToIO()
        from _1 in _schedulesRepository.UpdateAndSave(newSchedule)
        from _2 in _moduleCommandService.ChangeSchedule(module, newSchedule)
        select _2;
    
    private static Fin<DailySchedule> ToDailySchedule(SetDailyScheduleRequest request, Id<GrowHubModule> moduleId) =>
        from units in request.Entries.ToScheduleUnits(request.ScheduleId)
        from schedule in DailySchedule.New(units.ToImmutableList(), moduleId, request.ScheduleId)
        select schedule;
    
    private static Fin<WeeklySchedule> ToWeeklySchedule(SetWeeklyScheduleRequest request, Id<GrowHubModule> moduleId) =>
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