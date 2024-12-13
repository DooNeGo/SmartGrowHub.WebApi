using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Index(nameof(RefreshToken), IsUnique = true)]
internal sealed class UserSessionDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required Ulid UserId { get; set; }
    
    public UserDb? User { get; set; }

    public required string AccessToken { get; set; } = string.Empty;

    public required Ulid RefreshToken { get; set; }

    public required DateTime Expires { get; set; }
}