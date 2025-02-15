using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

namespace SmartGrowHub.Domain.Model.GrowHub.Components;

public sealed class HeaterComponent(Id<GrowHubComponent> id, ComponentProgram program) : GrowHubComponent(id, program);