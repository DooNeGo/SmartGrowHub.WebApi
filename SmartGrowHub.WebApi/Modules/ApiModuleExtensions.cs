using SmartGrowHub.WebApi.Modules.Auth;
using SmartGrowHub.WebApi.Modules.GrowHubs;
using SmartGrowHub.WebApi.Modules.Users;
using SmartGrowHub.WebApi.Modules.UserSessions;

namespace SmartGrowHub.WebApi.Modules;

public static class ApiModuleExtensions
{
    public static IEndpointRouteBuilder AddApiEndpoints(this IEndpointRouteBuilder routeBuilder) =>
        routeBuilder.MapGroup("/api")
            .AddAuthEndpoints()
            .AddUserEndpoints()
            .AddUserSessionEndpoints()
            .AddGrowHubEndpoints();
}
