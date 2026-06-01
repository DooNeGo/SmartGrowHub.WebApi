using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(SensorReadingConfiguration))]
internal sealed class SensorReadingDb : IContainsId
{
    public required string Id { get; set; }
    
    public required string SensorId { get; set; }

    public required SensorTypeDb Type { get; set; }

    public required QuantityDb Quantity { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required string GrowHubId { get; set; }

    public GrowHubDb? GrowHub { get; set; }
}
