using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(SensorReadingConfiguration))]
internal sealed class SensorReadingDb
{
    public required Ulid Id { get; set; }

    public required SensorTypeDb Type { get; set; }

    public required float Magnitude { get; set; }

    public required UnitDb Unit { get; set; }

    public required DateOnly CreatedAt { get; set; }

    public required Ulid GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}
