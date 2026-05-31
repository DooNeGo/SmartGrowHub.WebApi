using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleDb>
{
    public void Configure(EntityTypeBuilder<ScheduleDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Units)
            .WithOne(x => x.Schedule)
            .HasForeignKey(x => x.ScheduleId);
        
        builder.HasOne(x => x.GrowHubModule)
            .WithOne(x => x.Schedule)
            .HasForeignKey<ScheduleDb>(x => x.GrowHubModuleId);
    }
}