using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class ModuleProgramConfiguration : IEntityTypeConfiguration<ModuleProgramDb>
{
    public void Configure(EntityTypeBuilder<ModuleProgramDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Entries)
            .WithOne(x => x.ModuleProgram)
            .HasForeignKey(x => x.ModuleProgramId);
        
        builder.HasOne(x => x.GrowHubModule)
            .WithOne(x => x.Program)
            .HasForeignKey<ModuleProgramDb>(x => x.GrowHubModuleId);
    }
}