using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model;

/*
 * Value and Unit can be one Type.
 * For example, PowerValue, ModeValue ...
 * And it can validate itself
 */

public sealed class SettingComponent(
    Id<SettingComponent> id,
    SettingComponentType type,
    int value,
    NonEmptyString unit)
    : Entity<SettingComponent>(id)
{
    public SettingComponentType Type { get; init; } = type;

    public int Value { get; init; } = value;

    public NonEmptyString Unit { get; init; } = unit;
}