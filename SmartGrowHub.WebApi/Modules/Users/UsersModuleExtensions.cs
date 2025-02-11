using SmartGrowHub.WebApi.Modules.Users.Endpoints;

namespace SmartGrowHub.WebApi.Modules.Users;

public static class UsersModuleExtensions
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/user", GetUserEndpoint.GetUser).RequireAuthorization();

        return routeBuilder;
    }
}