using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Application.UseCases.GrowHubs;

namespace SmartGrowHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddTransient<RequestOtpToEmailUseCase>()
            .AddTransient<RequestOtpToPhoneUseCase>()
            .AddTransient<RefreshTokensUseCase>()
            .AddTransient<VerifyOtpUseCase>()
            .AddTransient<CreateGrowHubUseCase>()
            .AddTransient<UpdateScheduleUseCase>();
}