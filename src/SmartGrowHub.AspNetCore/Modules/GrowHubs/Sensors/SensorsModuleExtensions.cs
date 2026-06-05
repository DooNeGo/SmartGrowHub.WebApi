using SmartGrowHub.AspNetCore.Modules.GrowHubs.Sensors.Endpoints;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Sensors;

public static class SensorsModuleExtensions
{
    public static IEndpointRouteBuilder AddSensorsEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/{growHubId}/sensors/measurements/latest",
            GetLatestSensorsMeasurementsEndpoint.GetLatestSensorsMeasurements);

        return routeBuilder;
    }
}