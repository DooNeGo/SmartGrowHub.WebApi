using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiApplication(this IServiceCollection collection) =>
        collection.AddSingleton<TokenExpirationService>();
}
