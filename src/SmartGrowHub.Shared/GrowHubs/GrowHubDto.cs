namespace SmartGrowHub.Shared.GrowHubs;

public sealed record GrowHubDto(
    Ulid Id,
    string Name,
    string Model,
    PlantDto? Plant,
    IEnumerable<GrowHubModuleDto> Components);
