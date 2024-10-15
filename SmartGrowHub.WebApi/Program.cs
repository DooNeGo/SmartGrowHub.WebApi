using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.WebApi.Infrastructure;
using SmartGrowHub.WebApi.Infrastructure.Tokens;
using SmartGrowHub.WebApi.Modules;
using SmartGrowHub.WebApi.SerializerContext;
using SmartGrowHub.WebApi.Application;

namespace SmartGrowHub.WebApi;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain
                .Add(SmartGrowHubSerializerContext.Default);
        });

        AccessTokenConfiguration configuration = builder.Configuration
            .CreateAccessTokenConfiguration()
            .ThrowIfFail();

        builder.Services
            .AddInfrastructure()
            .AddWebApiApplication()
            .AddAuthentication(configuration)
            .AddAuthorization();

        WebApplication app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

#if DEBUG
        app.UseDeveloperExceptionPage();
#endif

        ApiModule.AddEndpointsTo(app);

        app.Run();
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