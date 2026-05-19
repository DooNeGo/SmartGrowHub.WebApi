using SmartGrowHub.AspNetCore.Modules.Auth;
using SmartGrowHub.AspNetCore.Modules.GrowHubs;
using SmartGrowHub.AspNetCore.Modules.Users;
using SmartGrowHub.AspNetCore.Modules.UserSessions;

namespace SmartGrowHub.AspNetCore.Modules;

public static class ApiModuleExtensions
{
    public static IEndpointRouteBuilder AddApiEndpoints(this IEndpointRouteBuilder routeBuilder) =>
        routeBuilder.MapGroup("/api")
            .AddAuthEndpoints()
            .AddUserEndpoints()
            .AddUserSessionEndpoints()
            .AddGrowHubEndpoints();
}
