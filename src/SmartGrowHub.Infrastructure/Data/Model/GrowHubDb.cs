using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(GrowHubDbConfiguration))]
internal sealed class GrowHubDb
{
    public required Ulid Id { get; set; }

    public required PlantDb? Plant { get; set; }

    public required Ulid UserId { get; set; }
    
    public UserDb? User { get; set; }

    public required List<SensorReadingDb> SensorReadings { get; set; } = [];

    public required List<SettingDb> Settings { get; set; } = [];
}
