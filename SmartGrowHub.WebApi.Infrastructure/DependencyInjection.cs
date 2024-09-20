using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Services;

namespace SmartGrowHub.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddDbContext<ApplicationContext>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IPasswordHasher, PasswordHasher>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IUserSessionService, UserSessionService>()
            .AddTransient<IUserSessionRepository, UserSessionRepository>();
}