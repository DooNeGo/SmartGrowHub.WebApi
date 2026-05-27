using SmartGrowHub.AspNetCore.Modules.GrowHubs.Endpoints;
using SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs;

public static class GrowHubModuleExtensions
{
    public static IEndpointRouteBuilder AddGrowHubEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/growHubs", GetGrowHubsEndpoint.GetGrowHubs).RequireAuthorization();
        
        routeBuilder.AddModulesEndpoints();
        
        return routeBuilder;
    }
}
