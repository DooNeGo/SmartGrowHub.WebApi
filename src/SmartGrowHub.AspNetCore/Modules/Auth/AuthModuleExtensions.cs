using SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

namespace SmartGrowHub.AspNetCore.Modules.Auth;

public static class AuthModuleExtensions
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder authGroup = routeBuilder.MapGroup("/auth");
        RouteGroupBuilder loginGroup = authGroup.MapGroup("/login");

        loginGroup.MapPost("/email", LogInByEmailEndpoint.LogIn);
        loginGroup.MapPost("/phone", LogInByPhoneEndpoint.LogIn);
        loginGroup.MapPost("/check", CheckOtpEndpoint.CheckOtp);
        //routeBuilder.MapPost("/auth/logout", LogOutEndpoint.LogOut);
        authGroup.MapPost("/refresh", RefreshTokensEndpoint.RefreshTokens);

        return routeBuilder;
    }
}
