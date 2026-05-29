using EntityFramework.Exceptions.PostgreSQL;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Infrastructure.Data;
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
                .AddMqttClient()
                .AddSingleton<ITokensIssuer, TokensIssuer>()
                .AddSingleton<IAccessTokenReader, AccessTokenReader>()
                .AddSingleton<ITimeProvider, TimeProvider>()
                .AddSingleton<IPasswordHasher, PasswordHasher>()
                .AddSingleton<ISmsService, SmsService>()
                .AddSingleton<IOtpIssuer, OtpIssuer>()
                .AddTransient<IEmailService, EmailService>()
                .AddSingleton<IFileService, FileService>()
                .AddTransient<ISmtpClient, SmtpClient>();

        private IServiceCollection AddDbContext(IConfiguration configuration) =>
            services.AddDbContextPool<ApplicationContext>(options => options
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                //.UseModel(ApplicationContextModel.Instance)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(
                    configuration.GetConnectionString("DatabaseConnection"),
                    sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseExceptionProcessor());

        private IServiceCollection AddRepositories() =>
            services
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUserSessionRepository, UserSessionRepository>()
                .AddTransient<IOtpRepository, OtpRepository>()
                .AddTransient<IGrowHubRepository, GrowHubRepository>()
                .AddTransient<IGrowHubModulesRepository, GrowHubModulesRepository>();

        private IServiceCollection AddMqttClient() =>
            services
                .AddSingleton<IMqttClient>(_ => new MqttClientFactory().CreateMqttClient())
                .AddSingleton<MqttClientOptions>(provider =>
                {
                    IConfigurationSection mqttSection = provider
                        .GetRequiredService<IConfiguration>()
                        .GetSection("Mqtt");

                    return new MqttClientOptionsBuilder()
                        .WithTcpServer(mqttSection["Host"], mqttSection.GetValue<int>("Port"))
                        .Build();
                })
                .AddSingleton<MqttClientDisconnectOptions>(_ => new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                    .Build());
    }
}