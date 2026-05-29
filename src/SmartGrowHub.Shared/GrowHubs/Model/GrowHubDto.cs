namespace SmartGrowHub.Shared.GrowHubs.Model;

public sealed record GrowHubDto(
    string Id,
    string Name,
    string Model,
    PlantDto? Plant,
    IReadOnlyList<GrowHubModuleDto> Modules);
