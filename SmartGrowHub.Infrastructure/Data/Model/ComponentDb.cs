using System.ComponentModel.DataAnnotations;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model;

internal sealed class ComponentDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required SettingComponentType Type { get; set; }

    public required int Value { get; set; }

    public required string Unit { get; set; }

    public required Ulid SettingId { get; set; }
    
    public SettingDb? Setting { get; set; }
}
