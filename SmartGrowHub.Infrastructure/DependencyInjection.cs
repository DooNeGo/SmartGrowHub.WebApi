using System.Net;
using System.Net.Mail;
using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.CompiledModels;
using SmartGrowHub.Infrastructure.Repositories;
using SmartGrowHub.Infrastructure.Services;
using SmartGrowHub.Infrastructure.Tokens;
using TimeProvider = SmartGrowHub.Infrastructure.Services.TimeProvider;

namespace SmartGrowHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext()
            .AddRepositories()
            .AddSingleton<ITokensIssuer, TokensIssuer>()
            .AddSingleton<IAccessTokenReader, AccessTokenReader>()
            .AddSingleton<ITimeProvider, TimeProvider>()
            .AddSingleton<IPasswordHasher, PasswordHasher>()
            .AddSingleton<ISmsService, SmsService>()
            .AddSingleton<IOtpIssuer, OtpIssuer>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<IUserService, UserService>()
            .AddFluentEmail(configuration);

    private static IServiceCollection AddDbContext(this IServiceCollection services) =>
        services.AddDbContextPool<ApplicationContext>(options => options
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseModel(ApplicationContextModel.Instance)
            .UseSqlite("DataSource=SmartGrowHubLocalDb", sqliteOptions => sqliteOptions
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .UseExceptionProcessor());

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IUserSessionRepository, UserSessionRepository>()
            .AddTransient<IOtpRepository, OtpRepository>();
    
    private static IServiceCollection AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
            .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"),
                configuration["Email:Username"], configuration["Email:Password"]);
        
        return services;
    }
}