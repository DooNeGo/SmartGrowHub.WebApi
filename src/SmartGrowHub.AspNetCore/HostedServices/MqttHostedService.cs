using System.Text.Json;
using MQTTnet;
using MQTTnet.Protocol;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.AspNetCore.HostedServices;

public sealed class MqttHostedService : IHostedService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private readonly MqttClientDisconnectOptions _disconnectOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITimeProvider _timeProvider;
    private readonly ILogger<MqttHostedService> _logger;
    private readonly Fin<NonEmptyString> _sensorsTopic;

    public MqttHostedService(
        IMqttClient mqttClient,
        MqttClientOptions options,
        MqttClientDisconnectOptions disconnectOptions,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ITimeProvider timeProvider,
        ILogger<MqttHostedService> logger)
    {
        _mqttClient = mqttClient;
        _options = options;
        _disconnectOptions = disconnectOptions;
        _serviceProvider = serviceProvider;
        _timeProvider = timeProvider;
        _logger = logger;
        
        _sensorsTopic = NonEmptyString
            .From(configuration["Mqtt:Topics:Sensors"]!)
            .MapFail(error =>
            {
                var newError = Error.New("Invalid MQTT sensors topic", error);
                logger.LogCritical(newError.ToException(), "Failed to read mqtt topics from configuration");
                return newError;
            });
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.ConnectAsync(_options, cancellationToken);
        await _mqttClient.SubscribeAsync("#", MqttQualityOfServiceLevel.AtMostOnce, cancellationToken);
        
        _mqttClient.ApplicationMessageReceivedAsync += args =>
        {
            string stringPayload = args.ApplicationMessage.ConvertPayloadToString();
            
            _logger.LogInformation("Received message with topic: {topic} and payload: {payload}",
                args.ApplicationMessage.Topic, stringPayload);

            return _sensorsTopic.Match(
                Succ: topic => args.ApplicationMessage.Topic.Contains(topic)
                    ? SaveSensorMeasurement(stringPayload, cancellationToken)
                        .RunSafeAsync()
                        .Map(fin => fin.MapFail(error =>
                        {
                            _logger.LogError(
                                error.ToException(),
                                "Failed to save sensor measurement with payload: {payload}", stringPayload);

                            return error;
                        }))
                        .ToRef()
                    : Task.CompletedTask,
                Fail: _ => Task.CompletedTask);
        };
    }

    private IO<Unit> SaveSensorMeasurement(string payload, CancellationToken cancellationToken) =>
        from utcNow in _timeProvider.UtcNow
        from measurements in IO.lift(() =>
        {
            var measurementsMqtt = JsonSerializer.Deserialize<SensorMeasurementsMqtt>(payload, JsonSerializerOptions);
            if (measurementsMqtt is null) return Iterable<SensorMeasurement>();

            return
                from growHubId in Domain.Common.Id<GrowHub>.From(measurementsMqtt.DeviceId)
                from measurements in measurementsMqtt.Data
                    .AsIterable()
                    .Traverse(mqtt => ToDomain(growHubId, utcNow, mqtt))
                    .As()
                select measurements;
        })
        from scope in use(IO.lift(() => _serviceProvider.CreateScope()))
        from repository in IO.lift(() => scope.ServiceProvider.GetRequiredService<ISensorMeasurementRepository>())
        from _1 in repository.AddRangeAndSave(measurements, cancellationToken)
        from _2 in release(scope)
        select _1;

    public Task StopAsync(CancellationToken cancellationToken) =>
        _mqttClient.DisconnectAsync(_disconnectOptions, cancellationToken);

    private static Fin<SensorMeasurement> ToDomain(Id<GrowHub> id, DateTime now, SensorMeasurementMqtt mqtt) =>
        from sensorId in NonEmptyString.From(mqtt.SensorId.ToString())
        select new SensorMeasurement(
            new Id<SensorMeasurement>(),
            id, sensorId, ToDomain(mqtt.Type),
            new Quantity(mqtt.Value, ToMeasurementUnit(mqtt.Unit)), now);
    
    private static MeasurementUnit ToMeasurementUnit(string unit) => unit switch
    {
        "C" => MeasurementUnit.Celsius,
        "%" => MeasurementUnit.Percent,
        "Pa" => MeasurementUnit.Pascals,
        "cm" => MeasurementUnit.Centimeters,
        "" => MeasurementUnit.Celsius,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
    
    private static SensorType ToDomain(string sensorType) => sensorType switch
    {
        "randomNumber" => SensorType.RandomNumber,
        "airTemperature" => SensorType.AirTemperature,
        "pressure" => SensorType.AirPressure,
        "airHumidity" => SensorType.AirHumidity,
        "plantHeight" => SensorType.PlantHeight,
        "soilMoisture" => SensorType.SoilMoisture,
        "soilTemperature" => SensorType.SoilTemperature,
        "light" => SensorType.Illumination,
        _ => throw new ArgumentOutOfRangeException(nameof(sensorType), $"Not expected sensor type value: {sensorType}")
    };
    
    private sealed record SensorMeasurementsMqtt(
        string DeviceId,
        IReadOnlyList<SensorMeasurementMqtt> Data);
    
    private sealed record SensorMeasurementMqtt(
        int SensorId,
        string Type,
        float Value,
        string Unit);
}