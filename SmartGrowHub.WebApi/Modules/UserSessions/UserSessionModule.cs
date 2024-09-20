using SmartGrowHub.WebApi.Modules.UserSessions.Endpoints;

namespace SmartGrowHub.WebApi.Modules.UserSessions;

public sealed class UserSessionModule : IEndpointModule
{
    public static IEndpointRouteBuilder AddEndpointsTo(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/auth/refresh", RefreshTokensEndpoint.RefreshTokens);

        return routeBuilder;
    }
}
