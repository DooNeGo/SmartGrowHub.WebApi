using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class GrowHubConfiguration : IEntityTypeConfiguration<GrowHubDb>
{
    public void Configure(EntityTypeBuilder<GrowHubDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Plant)
            .WithOne(x => x.GrowHub)
            .HasForeignKey<PlantDb>(x => x.GrowHubId);
        
        builder.HasMany(x => x.SensorReadings)
            .WithOne(x => x.GrowHub)
            .HasForeignKey(x => x.GrowHubId);
        
        builder.HasMany(x => x.Modules)
            .WithOne(x => x.GrowHub)
            .HasForeignKey(x => x.GrowHubId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}