using EntityFramework.Exceptions.PostgreSQL;
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
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration) =>
            services
                .AddDbContext(configuration)
                .AddRepositories()
                .AddSingleton<ITokensIssuer, TokensIssuer>()
                .AddSingleton<IAccessTokenReader, AccessTokenReader>()
                .AddSingleton<ITimeProvider, TimeProvider>()
                .AddSingleton<IPasswordHasher, PasswordHasher>()
                .AddSingleton<ISmsService, SmsService>()
                .AddSingleton<IOtpIssuer, OtpIssuer>()
                .AddTransient<IEmailService, EmailService>()
                .AddSingleton<IFileService, FileService>()
                .AddFluentEmail(configuration);

        private IServiceCollection AddDbContext(IConfiguration configuration) =>
            services.AddDbContextPool<ApplicationContext>(options => options
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseModel(ApplicationContextModel.Instance)
                .UseNpgsql(
                    configuration.GetConnectionString("DatabaseConnection"),
                    sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseExceptionProcessor());

        private IServiceCollection AddRepositories() =>
            services
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUserSessionRepository, UserSessionRepository>()
                .AddTransient<IOtpRepository, OtpRepository>();

        private IServiceCollection AddFluentEmail(IConfiguration configuration)
        {
            services
                .AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
                .AddSmtpSender(
                    configuration["Email:Host"],
                    configuration.GetValue<int>("Email:Port"),
                    configuration["Email:Username"],
                    configuration["Email:Password"]);
        
            return services;
        }
    }
}