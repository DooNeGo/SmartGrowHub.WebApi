
using SmartGrowHub.WebApi.Modules.Auth;
using SmartGrowHub.WebApi.Modules.Users;
using SmartGrowHub.WebApi.Modules.UserSessions;

namespace SmartGrowHub.WebApi.Modules;

public sealed class ApiModule : IEndpointModule
{
    public static IEndpointRouteBuilder AddEndpointsTo(
        IEndpointRouteBuilder routeBuilder) =>
            Id(routeBuilder.MapGroup("/api"))
                .Map(AuthModule.AddEndpointsTo)
                .Map(UsersModule.AddEndpointsTo)
                .Map(UserSessionModule.AddEndpointsTo)
                .Value;
}
