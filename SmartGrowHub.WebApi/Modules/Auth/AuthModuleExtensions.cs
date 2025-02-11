using SmartGrowHub.WebApi.Modules.Auth.Endpoints;

namespace SmartGrowHub.WebApi.Modules.Auth;

public static class AuthModuleExtensions
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/auth/login/email", LogInByEmailEndpoint.LogIn);
        routeBuilder.MapPost("/auth/login/phone", LogInByPhoneEndpoint.LogIn);
        routeBuilder.MapPost("/auth/login/check", CheckOtpEndpoint.CheckOtp);
        //routeBuilder.MapPost("/auth/logout", LogOutEndpoint.LogOut);
        routeBuilder.MapPost("/auth/refresh", RefreshTokensEndpoint.RefreshTokens);

        return routeBuilder;
    }
}
