using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public sealed class DisabledProgram(Id<ModuleProgram> id) : ModuleProgram(id)
{
    public static DisabledProgram New() => new(new Id<ModuleProgram>());
}