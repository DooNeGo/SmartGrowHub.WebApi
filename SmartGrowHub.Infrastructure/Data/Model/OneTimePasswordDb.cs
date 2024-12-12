using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Index(nameof(Value))]
internal sealed class OneTimePasswordDb
{
    [Key]
    public required Ulid Id { get; set; }
    
    public required Ulid UserDbId { get; set; }
    
    public required int Value { get; set; }
    
    public required DateTime Expires { get; set; }
}