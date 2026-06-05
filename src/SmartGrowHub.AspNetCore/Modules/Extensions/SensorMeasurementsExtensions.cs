using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

public static class SensorMeasurementsExtensions
{
    public static SensorMeasurementDto ToDto(this SensorMeasurement measurement) =>
        new(measurement.Id, measurement.SensorId, measurement.Type.ToDto(), measurement.Quantity.ToDto(),
            measurement.CreatedAt);

    public static SensorTypeDto ToDto(this SensorType type) => type switch
    {
        SensorType.RandomNumber => SensorTypeDto.RandomNumber,
        SensorType.AirTemperature => SensorTypeDto.AirTemperature,
        SensorType.AirPressure => SensorTypeDto.AirPressure,
        SensorType.AirHumidity => SensorTypeDto.AirHumidity,
        SensorType.PlantHeight => SensorTypeDto.PlantHeight,
        SensorType.SoilMoisture => SensorTypeDto.SoilMoisture,
        SensorType.SoilTemperature => SensorTypeDto.SoilTemperature,
        SensorType.Illumination => SensorTypeDto.Illumination,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}