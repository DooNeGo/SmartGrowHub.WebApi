using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Domain.Extensions;

public static class ModuleProgramExtensions
{
    extension(ModuleSchedule schedule)
    {
        public ProgramType Type => schedule.Match(
            _ => ProgramType.Disabled,
            _ => ProgramType.Manual,
            _ => ProgramType.Daily,
            _ => ProgramType.Weekly);
    }
}