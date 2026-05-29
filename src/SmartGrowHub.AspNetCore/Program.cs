using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.Application;
using SmartGrowHub.Application.Services;
using SmartGrowHub.AspNetCore.HostedServices;
using SmartGrowHub.AspNetCore.Services;
using SmartGrowHub.Infrastructure;
using SmartGrowHub.Infrastructure.Tokens;
using SmartGrowHub.Shared.SerializerContext;
using SmartGrowHub.AspNetCore.Modules;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.AspNetCore;

internal sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            JsonSerializerOptions serializerOptions = options.SerializerOptions;
            
            serializerOptions.TypeInfoResolverChain.Add(SmartGrowHubSerializerContext.Default);
            serializerOptions.Converters.Add(new JsonStringEnumConverter<ModuleTypeDto>());
            serializerOptions.Converters.Add(new JsonStringEnumConverter<ProgramTypeDto>());
        });
        
        AccessTokenConfiguration configuration = builder.Configuration
            .CreateAccessTokenConfiguration()
            .ThrowIfFail();

        builder.Services
            .AddOpenApi() 
            .AddSingleton<IEmailTemplateService, EmailTemplateService>()
            .AddHostedService<MqttHostedService>()
            .AddApplication()
            .AddInfrastructure(builder.Configuration)
            .AddAuthentication(configuration)
            .AddAuthorization();

        WebApplication app = builder.Build();
        
        app.UseAuthentication();
        app.UseAuthorization();

#if DEBUG
        app.UseDeveloperExceptionPage();
#endif

        app.MapOpenApi();

        app.AddApiEndpoints();
        
        await app.RunAsync();
    }
}

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services,
        AccessTokenConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.Issuer,
                    ValidAudience = configuration.Audience,
                    IssuerSigningKey = configuration.SigningCredentials.Key,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}