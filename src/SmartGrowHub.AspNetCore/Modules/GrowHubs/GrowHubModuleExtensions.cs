using SmartGrowHub.AspNetCore.Modules.GrowHubs.Endpoints;
using SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs;

public static class GrowHubModuleExtensions
{
    public static IEndpointRouteBuilder AddGrowHubEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder growHubsGroup = routeBuilder.MapGroup("/growHubs").RequireAuthorization();
        
        growHubsGroup.MapGet("", GetGrowHubsEndpoint.GetGrowHubs);
        growHubsGroup.MapPost("/register", RegisterGrowHubEndpoint.RegisterGrowHub);
            
        growHubsGroup.AddModulesEndpoints();
        
        return routeBuilder;
    }
}
