using System.Collections.Immutable;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Protocol;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.AspNetCore.HostedServices;

public sealed class MqttHostedService : IHostedService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    
    private static readonly JsonSerializerOptions MobileAppJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private readonly MqttClientDisconnectOptions _disconnectOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITimeProvider _timeProvider;
    private readonly ILogger<MqttHostedService> _logger;
    private readonly Fin<NonEmptyString> _growHubsSensorsTopic;
    private readonly Fin<NonEmptyString> _mobileAppsSensorsTopic;

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
        
        _growHubsSensorsTopic = NonEmptyString
            .From(configuration["Mqtt:Topics:GrowHub:Sensors"]!)
            .MapFail(error =>
            {
                var newError = Error.New("Invalid MQTT grow hub sensors topic", error);
                logger.LogCritical(newError.ToException(), "Failed to read mqtt topics from configuration");
                return newError;
            });
        
        _mobileAppsSensorsTopic = NonEmptyString
            .From(configuration["Mqtt:Topics:MobileApp:Sensors"]!)
            .MapFail(error =>
            {
                var newError = Error.New("Invalid MQTT mobile app sensors topic", error);
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

            return (
                    from topic in _growHubsSensorsTopic.ToIO()
                    from _ in args.ApplicationMessage.Topic.Contains(topic)
                        ? HandleSensors(stringPayload)
                        : IO.pure(unit)
                    select _)
                .RunSafeAsync(EnvIO.New(token: cancellationToken))
                .Map(fin => fin.MapFail(error =>
                {
                    _logger.LogError(
                        error.ToException(),
                        "Failed to save sensor measurement with payload: {payload}", stringPayload);

                    return error;
                }))
                .ToRef();
        };
    }

    private IO<Unit> HandleSensors(string payload) =>
        from utcNow in _timeProvider.UtcNow
        from measurements in IO.lift(() =>
        {
            var measurementsMqtt = JsonSerializer.Deserialize<SensorMeasurementsMqtt>(payload, JsonSerializerOptions);
            if (measurementsMqtt is null) return ImmutableList<SensorMeasurement>.Empty;

            return
                from growHubId in Domain.Common.Id<GrowHub>.From(measurementsMqtt.DeviceId)
                from measurements in measurementsMqtt.Data
                    .AsIterable()
                    .Traverse(mqtt => ToDomain(growHubId, utcNow, mqtt))
                    .As()
                select measurements.ToImmutableList();
        })
        from _1 in SaveSensorMeasurements(measurements)
        from _2 in PublishSensorMeasurementsToMobileApps(measurements)
        select _2;

    private IO<Unit> PublishSensorMeasurementsToMobileApps(ImmutableList<SensorMeasurement> measurements) =>
        from topic in _mobileAppsSensorsTopic.ToIO()
        from _ in measurements
            .AsIterable()
            .TraverseM(measurement => IO.liftAsync(async env =>
            {
                MqttApplicationMessage message = new MqttApplicationMessageBuilder()
                    .WithTopic($"{topic}/{measurement.SensorId}")
                    .WithPayload(JsonSerializer.Serialize(measurement.ToDto(), MobileAppJsonSerializerOptions))
                    .Build();

                await _mqttClient.PublishAsync(message, env.Token);
                return unit;
            }))
        select unit;

    private IO<Unit> SaveSensorMeasurements(ImmutableList<SensorMeasurement> measurements) =>
        from scope in use(IO.lift(() => _serviceProvider.CreateScope()))
        from repository in IO.lift(() => scope.ServiceProvider.GetRequiredService<ISensorMeasurementRepository>())
        from _1 in repository.AddRangeAndSave(measurements)
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
        "-" => MeasurementUnit.Celsius,
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