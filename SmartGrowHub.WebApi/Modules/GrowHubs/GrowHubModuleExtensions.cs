using SmartGrowHub.WebApi.Modules.GrowHubs.Endpoints;

namespace SmartGrowHub.WebApi.Modules.GrowHubs;

public static class GrowHubModuleExtensions
{
    public static IEndpointRouteBuilder AddGrowHubEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/growHubs", GetGrowHubsEndpoint.GetGrowHubs).RequireAuthorization();

        return routeBuilder;
    }
}
