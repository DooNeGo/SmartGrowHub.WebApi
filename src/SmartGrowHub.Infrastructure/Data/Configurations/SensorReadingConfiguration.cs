using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReadingDb>
{
    public void Configure(EntityTypeBuilder<SensorReadingDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.GrowHub)
            .WithMany(x => x.SensorReadings)
            .HasForeignKey(x => x.GrowHubId);

        builder.OwnsOne(x => x.Quantity, x =>
        {
            x.Property(p => p.Magnitude).HasColumnName("Value");
            x.Property(p => p.Unit).HasColumnName("Unit");;
        });
    }
}