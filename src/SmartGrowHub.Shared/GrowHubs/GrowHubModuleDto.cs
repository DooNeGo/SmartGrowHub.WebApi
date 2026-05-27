using SmartGrowHub.Shared.GrowHubs.ModulePrograms;

namespace SmartGrowHub.Shared.GrowHubs;

public sealed record GrowHubModuleDto(Ulid Id, ModuleProgramDto Program, ModuleTypeDto Type);