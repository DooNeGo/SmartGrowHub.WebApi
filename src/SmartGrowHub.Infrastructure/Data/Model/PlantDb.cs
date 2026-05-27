using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(PlantConfiguration))]
internal sealed class PlantDb
{
    public required Ulid Id { get; set; }
    
    public required string Name { get; set; } = string.Empty;
    
    public required Ulid GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}