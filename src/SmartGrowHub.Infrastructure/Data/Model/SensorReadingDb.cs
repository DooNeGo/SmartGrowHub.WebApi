using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(SensorReadingConfiguration))]
internal sealed class SensorReadingDb : IContainsId
{
    public required Ulid Id { get; set; }
    
    public required string SensorId { get; set; }

    public required SensorTypeDb Type { get; set; }

    public required QuantityDb Quantity { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required Ulid GrowHubId { get; set; }

    public GrowHubDb? GrowHub { get; set; }
}
