using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class GrowHubDbConfiguration : IEntityTypeConfiguration<GrowHubDb>
{
    public void Configure(EntityTypeBuilder<GrowHubDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.Settings)
            .WithOne(x => x.GrowHub)
            .HasForeignKey(x => x.GrowHubId);

        builder.HasMany(x => x.SensorReadings)
            .WithOne(x => x.GrowHub)
            .HasForeignKey(x => x.GrowHubId);
    }
}