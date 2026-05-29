using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class QuantityExtensions
{
    public static QuantityDb ToDb(this Quantity quantity) => new(quantity.Magnitude, quantity.Unit.ToDb());
    
    public static Quantity ToDomain(this QuantityDb quantity) => new(quantity.Magnitude, quantity.Unit.ToDomain());
}