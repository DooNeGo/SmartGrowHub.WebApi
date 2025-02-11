namespace SmartGrowHub.WebApi.Modules.UserSessions;

public static class UserSessionsModuleExtensions
{
    public static IEndpointRouteBuilder AddUserSessionEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        //routeBuilder.MapGet("/sessions", GetUserSessionsEndpoint.GetUserSessions);

        return routeBuilder;
    }
}
