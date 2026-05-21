namespace SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

public sealed record ManualProgramDto(Ulid Id, QuantityDto Quantity) : ComponentProgramDto(Id);
