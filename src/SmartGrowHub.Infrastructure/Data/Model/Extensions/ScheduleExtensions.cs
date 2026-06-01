using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class ScheduleExtensions
{
    public static ScheduleDb ToDb(this ModuleSchedule schedule)
    {
        var scheduleDb = new ScheduleDb
        {
            Id = schedule.Id,
            GrowHubModuleId = schedule.ModuleId,
            Type = ScheduleTypeDb.Disabled,
        };

        schedule.Match(
            _ => scheduleDb.Type = ScheduleTypeDb.Disabled,
            _ => scheduleDb.Type = ScheduleTypeDb.Enabled,
            daily =>
            {
                scheduleDb.Units = daily.Entries.Select(x => x.ToDb()).ToArray();
                return scheduleDb.Type = ScheduleTypeDb.Daily;
            },
            weekly =>
            {
                scheduleDb.Units = weekly.Entries.Select(x => x.ToDb()).ToArray();
                return scheduleDb.Type = ScheduleTypeDb.Weekly;
            });

        return scheduleDb;
    }

    public static Fin<ModuleSchedule> ToDomain(this ScheduleDb scheduleDb) =>
        from id in Id<ModuleSchedule>.From(scheduleDb.Id)
        from moduleId in Id<GrowHubModule>.From(scheduleDb.GrowHubModuleId)
        from schedule in scheduleDb.Type switch
        {
            ScheduleTypeDb.Disabled => Fin.Succ<ModuleSchedule>(new DisabledSchedule(id, moduleId)),
            ScheduleTypeDb.Enabled => Fin.Succ<ModuleSchedule>(new EnabledSchedule(id, moduleId)),
            ScheduleTypeDb.Daily => ToDailySchedule(scheduleDb, id, moduleId).Cast<DailySchedule, ModuleSchedule>(),
            ScheduleTypeDb.Weekly => ToWeeklySchedule(scheduleDb, id, moduleId).Cast<WeeklySchedule, ModuleSchedule>(),
            _ => throw new ArgumentOutOfRangeException(nameof(scheduleDb), scheduleDb.Type, null)
        }
        select schedule;

    private static Fin<DailySchedule> ToDailySchedule(ScheduleDb scheduleDb, Id<ModuleSchedule> id,
        Id<GrowHubModule> moduleId) =>
        from units in scheduleDb.Units
            .AsIterable()
            .Traverse(x => x.ToDailyScheduleUnit())
            .As()
        from schedule in DailySchedule.New(units.ToImmutableList(), moduleId, id)
        select schedule;

    private static Fin<WeeklySchedule> ToWeeklySchedule(ScheduleDb scheduleDb, Id<ModuleSchedule> id,
        Id<GrowHubModule> moduleId) =>
        from units in scheduleDb.Units
            .AsIterable()
            .Traverse(x => x.ToWeeklyScheduleUnit())
            .As()
        from schedule in WeeklySchedule.New(units.ToImmutableList(), moduleId, id)
        select schedule;
}