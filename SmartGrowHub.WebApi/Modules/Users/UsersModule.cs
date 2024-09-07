using SmartGrowHub.WebApi.Modules.Users.Endpoints;

namespace SmartGrowHub.WebApi.Modules.Users;

public sealed class UsersModule : IEndpointModule
{
    public static IEndpointRouteBuilder AddEndpointsTo(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/user", GetUserEndpoint.GetUser).RequireAuthorization();

        return routeBuilder;
    }
}