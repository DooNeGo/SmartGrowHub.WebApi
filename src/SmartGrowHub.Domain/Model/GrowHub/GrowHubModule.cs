using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Programs;

namespace SmartGrowHub.Domain.Model.GrowHub;

public sealed class GrowHubModule(
    Id<GrowHubModule> id,
    ModuleProgram program,
    ModuleType type)
    : Entity<GrowHubModule>(id)
{
    public ModuleProgram Program { get; } = program;

    public ModuleType Type { get; } = type;
    
    public static GrowHubModule New(ModuleProgram program, ModuleType type) =>
        new(new Id<GrowHubModule>(), program, type);
    
    public GrowHubModule SetProgram(ModuleProgram program) => new(Id, program, Type);
}