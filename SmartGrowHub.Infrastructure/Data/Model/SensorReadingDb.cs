using System.ComponentModel.DataAnnotations;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model;

internal sealed class SensorReadingDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required SensorType Type { get; set; }

    public required string Value { get; set; } = string.Empty;

    public required string Unit { get; set; } = string.Empty;

    public required DateOnly CreatedAt { get; set; }

    public required Ulid GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}
