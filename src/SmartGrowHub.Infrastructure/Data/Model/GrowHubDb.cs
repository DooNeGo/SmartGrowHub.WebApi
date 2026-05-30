using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(GrowHubConfiguration))]
internal sealed class GrowHubDb : IContainsId
{
    public required string Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Model { get; set; }
    
    public PlantDb? Plant { get; set; }

    public required string UserId { get; set; }
    
    public UserDb? User { get; set; }

    public ICollection<SensorReadingDb> SensorReadings { get; set; } = [];
    
    public ICollection<GrowHubModuleDb> Modules { get; set; } = [];
}
