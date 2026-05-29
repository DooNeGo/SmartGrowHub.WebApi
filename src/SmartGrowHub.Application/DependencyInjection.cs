using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Application.UseCases.GrowHubs;

namespace SmartGrowHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddTransient<SendOtpToEmailUseCase>()
            .AddTransient<SendOtpToPhoneUseCase>()
            .AddTransient<RefreshTokensUseCase>()
            .AddTransient<CheckOtpUseCase>()
            .AddTransient<RegisterGrowHubUseCase>()
            .AddTransient<SetModuleProgramUseCase>();
}