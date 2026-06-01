using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class MessageService : IMessageService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };
    
    private readonly IMqttClient _mqttClient;
    private readonly Fin<NonEmptyString> _modulesTopic;

    public MessageService(IMqttClient mqttClient, IConfiguration configuration, ILogger<MessageService> logger)
    {
        _mqttClient = mqttClient;
        _modulesTopic = NonEmptyString
            .From(configuration["Mqtt:Topics:Modules"]!)
            .MapFail(error =>
            {
                var newError = Error.New("Invalid MQTT modules topic", error);
                logger.LogCritical(newError.ToException(), "Failed to read mqtt topics from configuration");
                return newError;
            });
    }

    public IO<Unit> ChangeSchedule(GrowHubModule module, ModuleSchedule schedule, CancellationToken cancellationToken) =>
        from topic in _modulesTopic.ToIO()
        from _ in IO.liftAsync(async () =>
        {
            MqttApplicationMessageBuilder builder = new MqttApplicationMessageBuilder()
                .WithTopic(topic);

            var clearAllCommand = new ModuleCommandMqtt(
                module.GrowHubId.Value,
                Ulid.NewUlid().ToString(),
                ToMqtt(module.Type),
                ModuleModeMqtt.None,
                ScheduleActionMqtt.ClearAll,
                null);
            
            var changeModeCommand = new ModuleCommandMqtt(
                module.GrowHubId.Value,
                Ulid.NewUlid().ToString(),
                ToMqtt(module.Type),
                ToMqtt(schedule),
                ScheduleActionMqtt.None,
                null);
            
            MqttApplicationMessage clearAllMessage = builder
                .WithPayload(JsonSerializer.Serialize(clearAllCommand, JsonSerializerOptions))
                .Build();
            
            MqttApplicationMessage changeModeMessage = builder
                .WithPayload(JsonSerializer.Serialize(changeModeCommand, JsonSerializerOptions))
                .Build();
            
            await _mqttClient.PublishAsync(clearAllMessage,  cancellationToken);
            await _mqttClient.PublishAsync(changeModeMessage,  cancellationToken);

            await schedule.Match(
                _ => Task.FromResult(Unit.Default),
                _ => Task.FromResult(Unit.Default),
                async daily =>
                {
                    foreach (ScheduleUnit<TimeOnlyWrapper> unit in daily.Entries)
                    {
                        var addScheduleUnitCommand = new ModuleCommandMqtt(
                            module.GrowHubId.Value,
                            Ulid.NewUlid().ToString(),
                            ToMqtt(module.Type),
                            ModuleModeMqtt.None,
                            ScheduleActionMqtt.Add,
                            ToMqtt(unit));

                        MqttApplicationMessage addScheduleUnitMessage = builder
                            .WithPayload(JsonSerializer.Serialize(addScheduleUnitCommand, JsonSerializerOptions))
                            .Build();

                        await _mqttClient.PublishAsync(addScheduleUnitMessage, cancellationToken);
                    }

                    return Unit.Default;
                },
                async weekly =>
                {
                    foreach (ScheduleUnit<WeekTimeOnly> unit in weekly.Entries)
                    {
                        var addScheduleUnitCommand = new ModuleCommandMqtt(
                            module.GrowHubId.Value,
                            Ulid.NewUlid().ToString(),
                            ToMqtt(module.Type),
                            ModuleModeMqtt.None,
                            ScheduleActionMqtt.Add,
                            ToMqtt(unit));

                        MqttApplicationMessage addScheduleUnitMessage = builder
                            .WithPayload(JsonSerializer.Serialize(addScheduleUnitCommand, JsonSerializerOptions))
                            .Build();

                        await _mqttClient.PublishAsync(addScheduleUnitMessage, cancellationToken);
                    }

                    return Unit.Default;
                });
            
            return Unit.Default;
        })
        select _;

    private static ScheduleUnitMqtt ToMqtt(ScheduleUnit<TimeOnlyWrapper> unit) => new(
        unit.Id.Value,
        ToMqtt(unit.Kind),
        ToMqtt(unit.TimeInterval),
        ToMqtt(unit.Quantity));
    
    private static ScheduleUnitMqtt ToMqtt(ScheduleUnit<WeekTimeOnly> unit) => new(
        unit.Id.Value,
        ToMqtt(unit.Kind),
        ToMqtt(unit.TimeInterval),
        ToMqtt(unit.Quantity));

    private static QuantityMqtt ToMqtt(Quantity quantity) => new((int)quantity.Magnitude, ToMqtt(quantity.Unit));

    private static string ToMqtt(MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Percent => "%",
        MeasurementUnit.Celsius => "C"
    };
    
    private static IntervalMqtt ToMqtt(TimeInterval<TimeOnlyWrapper> interval) =>
        new(ToMqtt(interval.Start), ToMqtt(interval.End));
    
    private static IntervalMqtt ToMqtt(TimeInterval<WeekTimeOnly> interval) =>
        new(ToMqtt(interval.Start), ToMqtt(interval.End));

    private static string ToMqtt(TimeOnlyWrapper time) => $"01T{time.Inner:hh:mm}";
    
    private static string ToMqtt(WeekTimeOnly time) => $"{ToMqtt(time.DayOfWeek):D2}T{time.Time:hh:mm}";
    
    private static int ToMqtt(DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Monday => 1,
        DayOfWeek.Tuesday => 2,
        DayOfWeek.Wednesday => 3,
        DayOfWeek.Thursday => 4,
        DayOfWeek.Friday => 5,
        DayOfWeek.Saturday => 6,
        DayOfWeek.Sunday => 7
    };
    
    private static ScheduleKindMqtt ToMqtt(ScheduleUnitKind kind) => kind switch
    {
        ScheduleUnitKind.Power => ScheduleKindMqtt.Power,
        ScheduleUnitKind.Prefer => ScheduleKindMqtt.Prefer
    };

    private static ModuleModeMqtt ToMqtt(ModuleSchedule schedule) =>
        schedule.Match(
            _ => ModuleModeMqtt.Off,
            _ => ModuleModeMqtt.On,
            _ => ModuleModeMqtt.Daily,
            _ => ModuleModeMqtt.Weekly);
    
    private static ModuleTypeMqtt ToMqtt(ModuleType moduleType) => moduleType switch
    {
        ModuleType.Led => ModuleTypeMqtt.Led,
        ModuleType.DayLight => ModuleTypeMqtt.DayLight,
        ModuleType.UvLight => ModuleTypeMqtt.UvLight,
        ModuleType.AirFlap => ModuleTypeMqtt.AirFlap,
        ModuleType.Fan => ModuleTypeMqtt.Fan,
        ModuleType.Heater => ModuleTypeMqtt.Heater,
        ModuleType.Humidifier => ModuleTypeMqtt.Humidifier,
        ModuleType.WaterPump => ModuleTypeMqtt.WaterPump
    };
}

public sealed record ModuleCommandMqtt(
    string DeviceId,
    string MessageId,
    ModuleTypeMqtt Type,
    ModuleModeMqtt Mode,
    ScheduleActionMqtt Action,
    ScheduleUnitMqtt? ScheduleUnit);

public sealed record ScheduleUnitMqtt(
    string ScheduleUnitId,
    ScheduleKindMqtt Kind,
    IntervalMqtt Interval,
    QuantityMqtt Quantity);

public sealed record IntervalMqtt(
    string Start,
    string End);

public sealed record QuantityMqtt(
    int Magnitude,
    string Unit);

public enum ScheduleKindMqtt
{
    Power,
    Prefer
}

public enum ModuleTypeMqtt
{
    Led,
    DayLight,
    UvLight,
    Heater,
    Humidifier,
    Fan,
    WaterPump,
    AirFlap
}

public enum ModuleModeMqtt
{
    Off,
    On,
    Weekly,
    Daily,
    None
}

public enum ScheduleActionMqtt
{
    Add,
    Delete,
    ClearAll,
    None
}