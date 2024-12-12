using System.ComponentModel.DataAnnotations;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model;

internal sealed class SettingDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required SettingType Type { get; set; }

    public required SettingMode Mode { get; set; }

    public required Ulid GrowHubDbId { get; set; }

    public required List<ComponentDb> Components { get; set; } = [];
}
