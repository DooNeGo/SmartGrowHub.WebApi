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
}