using SmartGrowHub.Application.Repositories;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;


namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Sensors.Endpoints;

public sealed class GetLatestSensorsMeasurementsEndpoint
{
    public static ValueTask<IResult> GetLatestSensorsMeasurements(string growHubId,
        ISensorMeasurementRepository repository, ILogger<GetLatestSensorsMeasurementsEndpoint> logger,
        CancellationToken cancellationToken) => (
            from id in Domain.Common.Id<GrowHub>.From(growHubId).ToIO()
            from measurements in repository.GetLatestByGrowHubId(id)
            select measurements.Select(ToDto).AsEnumerable())
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            dto => Ok(Result.Success(dto)),
            error => HandleError(logger, error)));

    private static SensorMeasurementDto ToDto(SensorMeasurement measurement) =>
        new(measurement.Id, measurement.GrowHubId, ToDto(measurement.Type), measurement.Quantity.ToDto(),
            measurement.CreatedAt);

    private static SensorTypeDto ToDto(SensorType type) => type switch
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