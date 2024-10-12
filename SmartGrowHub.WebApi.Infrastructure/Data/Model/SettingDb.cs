using SmartGrowHub.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed class SettingDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required SettingType Type { get; set; }

    public required SettingMode Mode { get; set; }

    public required Ulid GrowHubDbId { get; set; }

    public required List<ComponentDb> Components { get; set; } = [];
}
