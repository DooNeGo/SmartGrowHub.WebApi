using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class DayLightComponent(Id<GrowHubComponent> id, ComponentProgram program) : GrowHubComponent(id, program);