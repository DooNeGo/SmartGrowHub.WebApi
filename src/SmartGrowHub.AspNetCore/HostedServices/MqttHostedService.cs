using MQTTnet;

namespace SmartGrowHub.AspNetCore.HostedServices;

public sealed class MqttHostedService : IHostedService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private readonly MqttClientDisconnectOptions _disconnectOptions;

    public MqttHostedService(
        IMqttClient mqttClient,
        MqttClientOptions options,
        MqttClientDisconnectOptions disconnectOptions)
    {
        _mqttClient = mqttClient;
        _options = options;
        _disconnectOptions = disconnectOptions;
    }

    public Task StartAsync(CancellationToken cancellationToken) =>
        _mqttClient.ConnectAsync(_options, cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken) =>
        _mqttClient.DisconnectAsync(_disconnectOptions, cancellationToken);
}