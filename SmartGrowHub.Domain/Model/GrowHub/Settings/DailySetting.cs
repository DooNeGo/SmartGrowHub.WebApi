using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Schedules;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed class DailySetting(Id<Setting> id, DailySchedule schedule) : Setting(id)
{
    public DailySchedule Schedule => schedule;
    
    public static DailySetting New(DailySchedule schedule) => new(new Id<Setting>(), schedule);
}