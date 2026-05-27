using MQTTnet;

namespace SmartGrowHub.Infrastructure.Services;

public sealed class MessageService
{
    private readonly IMqttClient _mqttClient;

    public MessageService(IMqttClient mqttClient)
    {
        _mqttClient = mqttClient;
    }

    public IO<Unit> SendCommand() => IO.pure(Unit.Default);
}

public sealed record ModuleCommandDto(
    string DeviceId,
    string MessageId,
    ModuleType Type,
    ModuleMode Mode,
    ScheduleAction Action,
    ScheduleUnitDto? ScheduleUnit);

public sealed record ScheduleUnitDto(
    string ScheduleUnitId,
    ScheduleKind Kind,
    IntervalDto Interval,
    QuantityDto Quantity);

public sealed record IntervalDto(
    string Start,
    string End);

public sealed record QuantityDto(
    int Magnitude,
    string Unit);

public enum ScheduleKind
{
    Power,
    Prefer
}

public enum ModuleType
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

public enum ModuleMode
{
    Off,
    On,
    Weekly,
    Daily,
    None
}

public enum ScheduleAction
{
    Add,
    Delete,
    ClearAll,
    None
}