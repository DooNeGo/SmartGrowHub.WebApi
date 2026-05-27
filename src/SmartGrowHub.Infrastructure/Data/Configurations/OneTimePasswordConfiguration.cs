using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class OneTimePasswordConfiguration : IEntityTypeConfiguration<OneTimePasswordDb>
{
    public void Configure(EntityTypeBuilder<OneTimePasswordDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value).HasMaxLength(6);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.OneTimePasswords)
            .HasForeignKey(x => x.UserId);
    }
}