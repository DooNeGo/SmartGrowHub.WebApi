
using SmartGrowHub.WebApi.Modules.UserSessions.Endpoints;

namespace SmartGrowHub.WebApi.Modules.UserSessions;

public sealed class UserSessionsModule : IEndpointModule
{
    public static IEndpointRouteBuilder AddEndpointsTo(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/sessions", GetUserSessionsEndpoint.GetUserSessions);

        return routeBuilder;
    }
}
