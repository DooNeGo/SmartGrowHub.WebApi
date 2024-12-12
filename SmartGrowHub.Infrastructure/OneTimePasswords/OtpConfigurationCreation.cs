using Microsoft.Extensions.Configuration;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.OneTimePasswords;

public static class OtpConfigurationCreation
{
    public static Fin<OtpConfiguration> CreateOtpConfiguration(this IConfiguration configuration) =>
        from length in NonNegativeInteger.From(configuration.GetValue<int>("Otp:Length"))
        let expiration = configuration.GetOtpLifeTime()
        select new OtpConfiguration(length, expiration);

    public static TimeSpan GetOtpLifeTime(this IConfiguration configuration) =>
        TimeSpan.FromMinutes(configuration.GetValue<int>("Otp:ExpirationInMinutes"));
}