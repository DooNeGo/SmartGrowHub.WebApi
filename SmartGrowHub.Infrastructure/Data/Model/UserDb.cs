using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(UserDbConfiguration))]
internal sealed class UserDb
{
    public required Ulid Id { get; set; }
    
    public required string EmailAddress { get; set; } = string.Empty;
    
    public required string PhoneNumber { get; set; } = string.Empty;

    public List<GrowHubDb> GrowHubs { get; set; } = [];

    public List<UserSessionDb> Sessions { get; set; } = [];
}