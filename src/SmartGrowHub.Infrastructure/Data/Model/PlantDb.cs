using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(PlantConfiguration))]
internal sealed class PlantDb : IContainsId
{
    public required string Id { get; set; }
    
    public required string Name { get; set; }
    
    public required DateTime PlantedAt { get; set; }
    
    public required string GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}