using SmartGrowHub.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed class ComponentDb
{
    [Key]
    public required Ulid Id { get; set; }

    public required ComponentType Type { get; set; }

    public required int Value { get; set; }

    public required string Unit { get; set; }

    public required Ulid SettingDbId { get; set; }
}
