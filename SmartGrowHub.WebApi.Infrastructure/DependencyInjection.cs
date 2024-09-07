using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.CompiledModels;
using SmartGrowHub.WebApi.Infrastructure.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Services;

namespace SmartGrowHub.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .ConfigureDbContextPool(configuration)
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IPasswordHasher, PasswordHasher>()
            .AddTransient<IUserRepository, UserRepository>();

    private static IServiceCollection ConfigureDbContextPool(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContextPool<ApplicationContext>(options => options
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseModel(ApplicationContextModel.Instance)
                .UseSqlServer(configuration["ConnectionStrings:MSSQLServer"],
                    providerOptions => providerOptions.EnableRetryOnFailure()));
}