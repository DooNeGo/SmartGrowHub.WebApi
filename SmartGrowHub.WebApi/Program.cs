using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.WebApi.Infrastructure;
using SmartGrowHub.WebApi.Modules;
using SmartGrowHub.WebApi.SerializerContext;
using System.Text;

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

        builder.Services
            .AddInfrastructure()
            .AddAuthentication(builder.Configuration)
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
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}