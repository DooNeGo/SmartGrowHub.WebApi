using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.UseCases.Auth;

namespace SmartGrowHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddTransient<OtpLoginUseCase>();
}