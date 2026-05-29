using SmartGrowHub.Domain.Model.GrowHub.Programs;

namespace SmartGrowHub.Domain.Extensions;

public static class ModuleProgramExtensions
{
    extension(ModuleProgram program)
    {
        public ProgramType Type => program.Match(
            _ => ProgramType.Disabled,
            _ => ProgramType.Manual,
            _ => ProgramType.Daily,
            _ => ProgramType.Weekly);
    }
}