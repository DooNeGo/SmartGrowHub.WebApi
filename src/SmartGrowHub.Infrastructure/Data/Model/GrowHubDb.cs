using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(GrowHubConfiguration))]
internal sealed class GrowHubDb
{
    public required Ulid Id { get; set; }
    
    public PlantDb? Plant { get; set; }

    public required Ulid UserId { get; set; }
    
    public UserDb? User { get; set; }

    public List<SensorReadingDb> SensorReadings { get; set; } = [];
    
    public List<GrowHubModuleDb> Modules { get; set; } = [];
}
