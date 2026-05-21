using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.Infrastructure.Data.Model;

internal sealed class PlantDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required string Name { get; set; } = string.Empty;
    
    public Ulid GrowHubId { get; set; }
}