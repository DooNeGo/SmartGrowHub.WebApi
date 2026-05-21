using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartGrowHub.Infrastructure.Data.Convertors;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Data.Configurations;

internal sealed class UserDbConfiguration : IEntityTypeConfiguration<UserDb>
{
    public void Configure(EntityTypeBuilder<UserDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmailAddress)
            .HasConversion<NullableStringConverter>()
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.PhoneNumber)
            .HasConversion<NullableStringConverter>()
            .HasMaxLength(20)
            .IsRequired(false);

        builder.HasIndex(x => x.EmailAddress).IsUnique();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        
        builder.HasMany(x => x.GrowHubs)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        
        builder.HasMany(x => x.Sessions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        
        builder.HasMany(x => x.OneTimePasswords)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}