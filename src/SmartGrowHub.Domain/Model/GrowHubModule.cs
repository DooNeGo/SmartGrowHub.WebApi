using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Domain.Model;

public sealed class GrowHubModule(
    Id<GrowHubModule> id,
    Id<GrowHub> growHubId,
    ModuleProgram program,
    ModuleType type)
    : Entity<GrowHubModule>(id)
{
    public Id<GrowHub> GrowHubId { get; } = growHubId;
    
    public ModuleProgram Program { get; } = program;

    public ModuleType Type { get; } = type;
    
    public static GrowHubModule New(Id<GrowHub> id, ModuleProgram program, ModuleType type) =>
        new(new Id<GrowHubModule>(), id, program, type);

    public GrowHubModule SetProgram(ModuleProgram program) => new(Id, GrowHubId, program, Type);
}