using System.Collections.Immutable;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        Converters =
        {
            new JsonStringEnumConverter<ScheduleActionMqtt>(),
            new JsonStringEnumConverter<ModuleModeMqtt>(),
            new JsonStringEnumConverter<ModuleTypeMqtt>(),
            new JsonStringEnumConverter<ScheduleKindMqtt>()
        }
    };

    private readonly IMqttClient _mqttClient;
    private readonly Fin<NonEmptyString> _modulesTopic;
    private readonly ILogger<MessageService> _logger;

    public MessageService(IMqttClient mqttClient, IConfiguration configuration, ILogger<MessageService> logger)
    {
        _mqttClient = mqttClient;
        _logger = logger;
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
        let messages = BuildMessages(module, schedule, topic)
        from _ in messages
            .AsIterable()
            .Traverse(message => IO.liftAsync(() => _mqttClient.PublishAsync(message, cancellationToken)))
            .As().ToUnit()
        select _;
    
    private static ImmutableList<MqttApplicationMessage> BuildMessages(
        GrowHubModule module, ModuleSchedule schedule, NonEmptyString topic)
    {
        ImmutableList<MqttApplicationMessage>.Builder messages = ImmutableList.CreateBuilder<MqttApplicationMessage>();
        
        messages.Add(CreateMessage(topic, new ModuleCommandMqtt(
            module.GrowHubId.Value,
            Ulid.NewUlid().ToString(),
            ToMqtt(module.Type),
            ModuleModeMqtt.None,
            ScheduleActionMqtt.ClearAll,
            null)));
        
        messages.Add(CreateMessage(topic, new ModuleCommandMqtt(
            module.GrowHubId.Value,
            Ulid.NewUlid().ToString(),
            ToMqtt(module.Type),
            ToMqtt(schedule),
            ScheduleActionMqtt.None,
            null)));

        messages.AddRange(
            schedule.Match(
                Disabled: _ => [],
                Enabled: _ => [],
                Daily: daily => CreateEntriesMessages(topic, module, daily.Entries),
                Weekly: weekly => CreateEntriesMessages(topic, module, weekly.Entries)));

        return messages.ToImmutable();
    }
    
    private static IEnumerable<MqttApplicationMessage> CreateEntriesMessages<T>(
        NonEmptyString topic, GrowHubModule module, ImmutableList<ScheduleUnit<T>> entries)
        where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan> =>
        entries.Select(entry => CreateEntryMessage(topic, module, entry));

    private static MqttApplicationMessage CreateMessage(NonEmptyString topic, ModuleCommandMqtt command) =>
        new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(JsonSerializer.Serialize(command, JsonSerializerOptions))
            .Build();

    private static MqttApplicationMessage CreateEntryMessage<T>(
        NonEmptyString topic, GrowHubModule module, ScheduleUnit<T> unit)
        where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan> =>
        CreateMessage(topic, new ModuleCommandMqtt(
            module.GrowHubId.Value,
            Ulid.NewUlid().ToString(),
            ToMqtt(module.Type),
            ModuleModeMqtt.None,
            ScheduleActionMqtt.Add,
            ToMqtt(unit)));

    private static ScheduleUnitMqtt ToMqtt<T>(ScheduleUnit<T> unit)
        where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan> => new(
        unit.Id.Value, ToMqtt(unit.Kind), ToMqtt(unit.TimeInterval), ToMqtt(unit.Quantity));

    private static QuantityMqtt ToMqtt(Quantity quantity) => new((int)quantity.Magnitude, ToMqtt(quantity.Unit));

    private static string ToMqtt(MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Percent => "%",
        MeasurementUnit.Celsius => "C",
        MeasurementUnit.Centimeters => "cm",
        MeasurementUnit.Pascals => "Pa",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };

    private static IntervalMqtt ToMqtt<T>(TimeInterval<T> interval)
        where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan> =>
        interval switch
        {
            TimeInterval<TimeOnlyWrapper> dailyInterval =>
                new IntervalMqtt(ToMqtt(dailyInterval.Start), ToMqtt(dailyInterval.End)),
            TimeInterval<WeekTimeOnly> weeklyInterval =>
                new IntervalMqtt(ToMqtt(weeklyInterval.Start), ToMqtt(weeklyInterval.End)),
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };

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
        DayOfWeek.Sunday => 7,
        _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
    };

    private static ScheduleKindMqtt ToMqtt(ScheduleUnitKind kind) => kind switch
    {
        ScheduleUnitKind.Power => ScheduleKindMqtt.Power,
        ScheduleUnitKind.Prefer => ScheduleKindMqtt.Prefer,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };

    private static ModuleModeMqtt ToMqtt(ModuleSchedule schedule) =>
        schedule.Match(
            Disabled: _ => ModuleModeMqtt.Off,
            Enabled: _ => ModuleModeMqtt.On,
            Daily: _ => ModuleModeMqtt.Daily,
            Weekly: _ => ModuleModeMqtt.Weekly);

    private static ModuleTypeMqtt ToMqtt(ModuleType type) => type switch
    {
        ModuleType.Led => ModuleTypeMqtt.Led,
        ModuleType.DayLight => ModuleTypeMqtt.DayLight,
        ModuleType.UvLight => ModuleTypeMqtt.UvLight,
        ModuleType.AirFlap => ModuleTypeMqtt.AirFlap,
        ModuleType.Fan => ModuleTypeMqtt.Fan,
        ModuleType.Heater => ModuleTypeMqtt.Heater,
        ModuleType.Humidifier => ModuleTypeMqtt.Humidifier,
        ModuleType.WaterPump => ModuleTypeMqtt.WaterPump,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}

public sealed record ModuleCommandMqtt(
    string DeviceId, string MessageId, ModuleTypeMqtt Type,
    ModuleModeMqtt Mode, ScheduleActionMqtt Action, ScheduleUnitMqtt? ScheduleUnit);

public sealed record ScheduleUnitMqtt(
    string ScheduleUnitId, ScheduleKindMqtt Kind,
    IntervalMqtt Interval, QuantityMqtt Quantity);

public sealed record IntervalMqtt(string Start, string End);

public sealed record QuantityMqtt(int Magnitude, string Unit);

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