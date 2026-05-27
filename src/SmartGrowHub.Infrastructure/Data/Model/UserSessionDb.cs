using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(UserSessionConfiguration))]
internal sealed class UserSessionDb
{
    public required Ulid Id { get; set; }

    public required string AccessToken { get; set; } = string.Empty;

    public required Ulid RefreshToken { get; set; }

    public required DateTime Expires { get; set; }
    
    public required Ulid UserId { get; set; }
    
    public UserDb? User { get; set; }
}