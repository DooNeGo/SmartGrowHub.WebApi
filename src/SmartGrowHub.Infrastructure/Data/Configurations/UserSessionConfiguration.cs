using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSessionDb>
{
    public void Configure(EntityTypeBuilder<UserSessionDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.RefreshToken).IsUnique();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.UserId);
    }
}