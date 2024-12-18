
using SmartGrowHub.WebApi.Modules.GrowHubs.Endpoints;

namespace SmartGrowHub.WebApi.Modules.GrowHubs;

public sealed class GrowHubModule : IEndpointModule
{
    public static IEndpointRouteBuilder AddEndpointsTo(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/growHubs", GetGrowHubsEndpoint.GetGrowHubs).RequireAuthorization();

        return routeBuilder;
    }
}
