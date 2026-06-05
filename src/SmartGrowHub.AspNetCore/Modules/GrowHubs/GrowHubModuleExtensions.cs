using SmartGrowHub.AspNetCore.Modules.GrowHubs.Endpoints;
using SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules;
using SmartGrowHub.AspNetCore.Modules.GrowHubs.Sensors;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs;

public static class GrowHubModuleExtensions
{
    public static IEndpointRouteBuilder AddGrowHubEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder growHubsGroup = routeBuilder.MapGroup("/grow-hubs").RequireAuthorization();
        
        growHubsGroup.MapGet("", GetGrowHubsEndpoint.GetGrowHubs);
        growHubsGroup.MapPost("", CreateGrowHubEndpoint.CreateGrowHub);
            
        growHubsGroup
            .AddModulesEndpoints()
            .AddSensorsEndpoints();
        
        return routeBuilder;
    }
}
