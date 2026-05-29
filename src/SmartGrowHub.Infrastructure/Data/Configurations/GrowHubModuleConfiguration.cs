using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class GrowHubModuleConfiguration : IEntityTypeConfiguration<GrowHubModuleDb>
{
    public void Configure(EntityTypeBuilder<GrowHubModuleDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.GrowHub)
            .WithMany(x => x.Modules)
            .HasForeignKey(x => x.GrowHubId);
        
        builder.HasOne(x => x.Program)
            .WithOne(x => x.GrowHubModule)
            .HasForeignKey<ModuleProgramDb>(x => x.GrowHubModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}