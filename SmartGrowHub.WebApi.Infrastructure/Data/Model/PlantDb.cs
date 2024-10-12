using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed class PlantDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required string Name { get; set; } = string.Empty;
}