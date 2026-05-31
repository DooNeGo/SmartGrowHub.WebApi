using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class DisabledSchedule(Id<ModuleSchedule> id, Id<GrowHubModule> moduleId) : ModuleSchedule(id, moduleId)
{
    public static DisabledSchedule New(Id<GrowHubModule> moduleId) => new(new Id<ModuleSchedule>(), moduleId);
}