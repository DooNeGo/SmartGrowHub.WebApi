using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Domain.Model;

public sealed class GrowHubModule(
    Id<GrowHubModule> id,
    Id<GrowHub> growHubId,
    ModuleSchedule schedule,
    ModuleType type)
    : Entity<GrowHubModule>(id)
{
    public Id<GrowHub> GrowHubId { get; } = growHubId;
    
    public ModuleSchedule Schedule { get; } = schedule;

    public ModuleType Type { get; } = type;
    
    public static GrowHubModule New(Id<GrowHub> id, ModuleSchedule schedule, ModuleType type) =>
        new(new Id<GrowHubModule>(), id, schedule, type);

    public GrowHubModule SetProgram(ModuleSchedule schedule) => new(Id, GrowHubId, schedule, Type);
}