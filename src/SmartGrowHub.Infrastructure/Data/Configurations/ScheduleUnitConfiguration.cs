using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class ScheduleUnitConfiguration : IEntityTypeConfiguration<ScheduleUnitDb>
{
    public void Configure(EntityTypeBuilder<ScheduleUnitDb> builder)
    {
        builder.HasKey(s => s.Id);

        builder.OwnsOne(s => s.Quantity);
        builder.OwnsOne(s => s.Interval);
        
        builder.HasOne(x => x.Schedule)
            .WithMany(x => x.Units)
            .HasForeignKey(x => x.ScheduleId);
    }
}