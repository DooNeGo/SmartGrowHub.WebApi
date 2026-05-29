using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Owned]
internal sealed class TimedQuantityDb<TTime>
{
    public required QuantityDb Quantity { get; set; }
    
    public required IntervalDb<TTime> Interval { get; set; }
}