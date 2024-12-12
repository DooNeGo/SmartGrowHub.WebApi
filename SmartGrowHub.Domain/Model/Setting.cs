using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Model;

public sealed class Setting(
    Id<Setting> id,
    SettingType type,
    SettingMode mode,
    ImmutableDictionary<Id<SettingComponent>, SettingComponent> components)
    : Entity<Setting>(id)
{
    private Setting(Setting original) : this(
        original.Id, original.Type,
        original.Mode, original.Components)
    { }

    public SettingType Type { get; init; } = type;

    public SettingMode Mode { get; init; } = mode;

    public ImmutableDictionary<Id<SettingComponent>, SettingComponent> Components { get; init; } = components;

    public Setting AddComponent(SettingComponent component) =>
        new(this) { Components = Components.Add(component.Id, component) };

    public Setting RemoveComponent(Id<SettingComponent> id) =>
        new(this) { Components = Components.Remove(id) };
}