using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(EmailAddress), IsUnique = true)]
internal sealed class UserDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required string UserName { get; set; } = string.Empty;

    public required byte[] Password { get; set; } = [];

    public required string EmailAddress { get; set; } = string.Empty;

    public required string DisplayName { get; set; } = string.Empty;

    public List<GrowHubDb> GrowHubs { get; set; } = [];

    public List<UserSessionDb> Sessions { get; set; } = [];
}