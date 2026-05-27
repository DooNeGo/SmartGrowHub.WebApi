using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class PlantConfiguration : IEntityTypeConfiguration<PlantDb>
{
    public void Configure(EntityTypeBuilder<PlantDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.GrowHub)
            .WithOne(x => x.Plant)
            .HasForeignKey<PlantDb>(x => x.GrowHubId);
    }
}