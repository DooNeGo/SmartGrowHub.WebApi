using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class ModuleProgramConfiguration : IEntityTypeConfiguration<ModuleProgramDb>
{
    public void Configure(EntityTypeBuilder<ModuleProgramDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.ManualQuantity, x =>
        {
            x.Property(p => p.Magnitude).HasColumnName("ManualProgram_Magnitude");
            x.Property(p => p.Unit).HasColumnName("ManualProgram_Unit");
        });
        builder.OwnsMany(x => x.TimeOnlyEntries, x => x.ToJson());
        builder.OwnsMany(x => x.WeekTimeOnlyEntries, x => x.ToJson());
        
        builder.HasOne(x => x.GrowHubModule)
            .WithOne(x => x.Program)
            .HasForeignKey<ModuleProgramDb>(x => x.GrowHubModuleId);
    }
}