using SmartGrowHub.Shared.GrowHubs.Components;

namespace SmartGrowHub.Shared.GrowHubs;

public sealed record GrowHubDto(
    Ulid Id,
    string Name,
    string Model,
    PlantDto? Plant,
    IEnumerable<GrowHubComponentDto> Components);
