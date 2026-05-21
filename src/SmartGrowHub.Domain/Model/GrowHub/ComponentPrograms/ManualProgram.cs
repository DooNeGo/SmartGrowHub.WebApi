using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

public sealed class ManualProgram(Id<ComponentProgram> id, Quantity quantity) : ComponentProgram(id)
{
    public Quantity Quantity { get; } = quantity;
    
    public static ManualProgram New(Quantity quantity) => new(new Id<ComponentProgram>(), quantity);
}