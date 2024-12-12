using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model;

public sealed class SensorMeasurement(
    Id<SensorMeasurement> id,
    SensorType type,
    NonEmptyString value,
    NonEmptyString unit,
    DateTime createdAt)
    : Entity<SensorMeasurement>(id)
{
    public SensorType Type { get; init; } = type;

    public NonEmptyString Value { get; init; } = value;

    public NonEmptyString Unit { get; init; } = unit;

    public DateTime CreatedAt { get; init; } = createdAt;
}