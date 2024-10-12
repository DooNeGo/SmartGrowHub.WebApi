using SmartGrowHub.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed class SensorReadingDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required SensorType Type { get; set; }

    public required string Value { get; set; } = string.Empty;

    public required string Unit { get; set; } = string.Empty;

    public required DateOnly CreatedAt { get; set; }

    public required Ulid GrowHubDbId { get; set; }
}
