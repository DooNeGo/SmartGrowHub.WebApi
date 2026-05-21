using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(SettingDbConfiguration))]
internal sealed class SettingDb
{
    public required Ulid Id { get; set; }

    public required Ulid GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}
