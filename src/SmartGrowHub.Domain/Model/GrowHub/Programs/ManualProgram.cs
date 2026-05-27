using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public sealed class ManualProgram(Id<ModuleProgram> id, Quantity quantity) : ModuleProgram(id)
{
    public Quantity Quantity { get; } = quantity;
    
    public static ManualProgram New(Quantity quantity) => new(new Id<ModuleProgram>(), quantity);
}