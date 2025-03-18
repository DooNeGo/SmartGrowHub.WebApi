using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.Infrastructure.Data.Model;

internal sealed class OneTimePasswordDb
{
    [Key]
    public required Ulid Id { get; set; }
    
    public required Ulid UserId { get; set; }
    
    public UserDb? User { get; set; }
    
    [MaxLength(6)]
    public required string Value { get; set; }
    
    public required DateTime Expires { get; set; }
}