using SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules.Endpoints;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules;

internal static class ModulesModuleExtensions
{
    public static IEndpointRouteBuilder AddModulesEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder modulesGroup = routeBuilder.MapGroup("/modules/{moduleId}");

        modulesGroup.MapPost("/set-program", SetModuleProgramEndpoint.SetModuleProgram);
        
        return routeBuilder;
    }
}