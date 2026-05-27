namespace SmartGrowHub.Shared.GrowHubs.ModulePrograms;

public sealed record ManualProgramDto(Ulid Id, QuantityDto Quantity) : ModuleProgramDto(Id);
