using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public sealed class CycleProgram(Id<ModuleProgram> id, TimedQuantity<TimeOnlyWrapper> cycleParameters)
    : ModuleProgram(id)
{
    public TimedQuantity<TimeOnlyWrapper> CycleParameters { get; } = cycleParameters;
    
    public static CycleProgram New(TimedQuantity<TimeOnlyWrapper> interval) =>
        new(new Id<ModuleProgram>(), interval);
}