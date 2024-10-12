using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed class GrowHubDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required PlantDb? Plant { get; set; }

    public required Ulid UserDbId { get; set; }

    public required List<SensorReadingDb> SensorReadings { get; set; } = [];

    public required List<SettingDb> Settings { get; set; } = [];
}
