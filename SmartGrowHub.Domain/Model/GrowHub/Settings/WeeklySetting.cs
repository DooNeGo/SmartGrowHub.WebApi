using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Schedules;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed class WeeklySetting(Id<Setting> id, WeeklySchedule schedule) : Setting(id)
{
    public WeeklySchedule Schedule => schedule;
    
    public static WeeklySetting New(WeeklySchedule schedule) => new(new Id<Setting>(), schedule);
}