using SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

namespace SmartGrowHub.AspNetCore.Modules.Auth;

public static class AuthModuleExtensions
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder authGroup = routeBuilder.MapGroup("/auth");
        RouteGroupBuilder otpGroup = authGroup.MapGroup("/otp");

        otpGroup.MapPost("/email", RequestOtpToEmailEndpoint.Request);
        otpGroup.MapPost("/phone", RequestOtpToPhoneEndpoint.Request);
        otpGroup.MapPost("/verify", VerifyOtpEndpoint.Verify);
        //routeBuilder.MapPost("/auth/logout", LogOutEndpoint.LogOut);
        authGroup.MapPost("/tokens/refresh", RefreshTokensEndpoint.Refresh);

        return routeBuilder;
    }
}
