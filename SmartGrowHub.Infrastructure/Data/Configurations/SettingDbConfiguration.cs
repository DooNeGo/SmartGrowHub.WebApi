using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class SettingDbConfiguration : IEntityTypeConfiguration<SettingDb>
{
    public void Configure(EntityTypeBuilder<SettingDb> builder)
    {
        builder.HasKey(x => x.Id);
    }
}