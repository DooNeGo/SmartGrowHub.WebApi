using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class TimedQuantityExtensions
{
    public static TimedQuantityDb<TimeOnly> ToDb(this TimedQuantity<TimeOnlyWrapper> timedQuantity) => new()
    {
        Quantity = timedQuantity.Quantity.ToDb(),
        Interval = timedQuantity.TimeInterval.ToDb()
    };
    
    public static TimedQuantity<TimeOnlyWrapper> ToDomain(this TimedQuantityDb<TimeOnly> timedQuantity) =>
        new(timedQuantity.Quantity.ToDomain(), timedQuantity.Interval.ToDomain());

    public static TimedQuantityDb<WeekTimeOnlyDb> ToDb(this TimedQuantity<WeekTimeOnly> timedQuantity) => new()
    {
        Quantity = timedQuantity.Quantity.ToDb(),
        Interval = timedQuantity.TimeInterval.ToDb()
    };
    
    public static TimedQuantity<WeekTimeOnly> ToDomain(this TimedQuantityDb<WeekTimeOnlyDb> timedQuantity) =>
        new(timedQuantity.Quantity.ToDomain(), timedQuantity.Interval.ToDomain());
}