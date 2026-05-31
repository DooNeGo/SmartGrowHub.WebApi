using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class EnabledSchedule(Id<ModuleSchedule> id, Id<GrowHubModule> moduleId) : ModuleSchedule(id, moduleId)
{
    public static EnabledSchedule New(Id<GrowHubModule> moduleId) => new(new Id<ModuleSchedule>(), moduleId);
}