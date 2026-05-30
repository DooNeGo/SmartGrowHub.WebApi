using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(OneTimePasswordConfiguration))]
internal sealed class OneTimePasswordDb : IContainsId
{
    public required string Id { get; set; }
    
    public required string Value { get; set; }
    
    public required DateTime Expires { get; set; }
    
    public required string UserId { get; set; }
    
    public UserDb? User { get; set; }
}