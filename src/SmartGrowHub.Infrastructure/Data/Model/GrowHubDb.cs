using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(GrowHubConfiguration))]
internal sealed class GrowHubDb
{
    public required string Id { get; set; }
    
    public required string Name { get; set; } = string.Empty;
    
    public required string Model { get; set; } = string.Empty;
    
    public PlantDb? Plant { get; set; }

    public required string UserId { get; set; }
    
    public UserDb? User { get; set; }

    public List<SensorReadingDb> SensorReadings { get; set; } = [];
    
    public List<GrowHubModuleDb> Modules { get; set; } = [];
}
