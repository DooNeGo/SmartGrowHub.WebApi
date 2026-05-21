using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

public sealed class CycleProgram(Id<ComponentProgram> id, TimedQuantity<TimeOnlyWrapper> cycleParameters)
    : ComponentProgram(id)
{
    public TimedQuantity<TimeOnlyWrapper> CycleParameters { get; } = cycleParameters;
    
    public static CycleProgram New(TimedQuantity<TimeOnlyWrapper> interval) =>
        new(new Id<ComponentProgram>(), interval);
}