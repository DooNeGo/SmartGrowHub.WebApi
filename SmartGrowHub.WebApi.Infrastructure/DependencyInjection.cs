using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.Services;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.CompiledModels;
using SmartGrowHub.WebApi.Infrastructure.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Services;
using TimeProvider = SmartGrowHub.WebApi.Infrastructure.Services.TimeProvider;

namespace SmartGrowHub.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddDbContextPool<ApplicationContext>(options => options
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseModel(ApplicationContextModel.Instance)
                .UseSqlite("DataSource=SmartGrowHubLocalDb", options => options
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseExceptionProcessor())
            .AddSingleton<ITokenService, TokenService>()
            .AddSingleton<ITimeProvider, TimeProvider>()
            .AddSingleton<IPasswordHasher, PasswordHasher>()
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IUserSessionRepository, UserSessionRepository>();
}