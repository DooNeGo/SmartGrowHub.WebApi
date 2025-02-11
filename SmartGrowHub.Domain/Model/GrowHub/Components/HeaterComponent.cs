﻿using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Settings;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class HeaterComponent(
    Id<HeaterComponent> id,
    Setting setting)
    : Entity<HeaterComponent>(id)
{
    public Setting Setting { get; } = setting;
}
