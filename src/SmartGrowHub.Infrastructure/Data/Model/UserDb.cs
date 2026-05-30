using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(UserConfiguration))]
internal sealed class UserDb : IContainsId
{
    public required string Id { get; set; }
    
    public required string EmailAddress { get; set; } = string.Empty;
    
    public required string PhoneNumber { get; set; } = string.Empty;

    public ICollection<GrowHubDb> GrowHubs { get; set; } = [];

    public ICollection<UserSessionDb> Sessions { get; set; } = [];
    
    public ICollection<OneTimePasswordDb> OneTimePasswords { get; set; } = [];
}