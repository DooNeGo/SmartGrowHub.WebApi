using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(ScheduleUnitConfiguration))]
internal sealed class ScheduleUnitDb : IContainsId
{
    public required Ulid Id { get; set; }
    
    public required ScheduleUnitKindDb Kind { get; set; }
    
    public required QuantityDb Quantity { get; set; }
    
    public required IntervalDb Interval { get; set; }
    
    public required Ulid ScheduleId { get; set; } 
    
    public ScheduleDb? Schedule { get; set; }
}