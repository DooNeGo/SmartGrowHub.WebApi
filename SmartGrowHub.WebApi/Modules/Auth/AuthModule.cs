using SmartGrowHub.WebApi.Modules.Auth.Endpoints;

namespace SmartGrowHub.WebApi.Modules.Auth;

public sealed class AuthModule : IEndpointModule
{
    public static IEndpointRouteBuilder AddEndpointsTo(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/auth/login", LogInEndpoint.LogIn);
        routeBuilder.MapPost("/auth/register", RegisterEndpoint.Register);

        return routeBuilder;
    }
}
