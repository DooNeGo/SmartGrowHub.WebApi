using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class TimedQuantityConfiguration : IEntityTypeConfiguration<TimedQuantityDb>
{
    public void Configure(EntityTypeBuilder<TimedQuantityDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.ModuleProgram)
            .WithMany(x => x.Entries)
            .HasForeignKey(x => x.ModuleProgramId);
    }
}