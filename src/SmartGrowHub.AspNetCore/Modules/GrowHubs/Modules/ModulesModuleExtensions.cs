namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules;

public static class ModulesModuleExtensions
{
    public static IEndpointRouteBuilder AddModulesEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder modulesGroup = routeBuilder.MapGroup("/growHubs/modules/{id}");

        //modulesGroup.MapPut();
        
        return routeBuilder;
    }
}