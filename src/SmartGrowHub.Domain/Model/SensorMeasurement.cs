using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model;

public sealed class SensorMeasurement(
    Id<SensorMeasurement> id,
    Id<GrowHub> growHubId,
    NonEmptyString sensorId,
    SensorType type,
    Quantity quantity,
    DateTime createdAt)
    : Entity<SensorMeasurement>(id)
{
    public Id<GrowHub> GrowHubId { get; init; } = growHubId;
    
    public NonEmptyString SensorId { get; init; } = sensorId;
    
    public SensorType Type { get; init; } = type;

    public Quantity Quantity { get; init; } = quantity;

    public DateTime CreatedAt { get; init; } = createdAt;
}