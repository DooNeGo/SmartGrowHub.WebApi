using SmartGrowHub.AspNetCore.Modules.Users.Endpoints;

namespace SmartGrowHub.AspNetCore.Modules.Users;

public static class UsersModuleExtensions
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/user", GetUserEndpoint.GetUser).RequireAuthorization();

        return routeBuilder;
    }
}