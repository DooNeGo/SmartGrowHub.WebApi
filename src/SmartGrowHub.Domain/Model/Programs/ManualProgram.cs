using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class ManualProgram : ModuleProgram
{
    private ManualProgram(Id<ModuleProgram> id, Quantity quantity) : base(id) => Quantity = quantity;

    public Quantity Quantity { get; }
    
    public Fin<ManualProgram> ChangeQuantity(Quantity quantity) => New(quantity, Id);
    
    public static Fin<ManualProgram> New(Quantity quantity, Id<ModuleProgram>? id = null) =>
        from _ in QuantityDefaults.ValidatePowerPercentRange(quantity)
        select new ManualProgram(id ?? new Id<ModuleProgram>(), quantity);
}