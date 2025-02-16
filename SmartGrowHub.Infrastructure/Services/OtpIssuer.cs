using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.OneTimePasswords;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed partial class OtpIssuer(
    ITimeProvider timeProvider,
    IConfiguration configuration,
    ILogger<OtpIssuer> logger)
    : IOtpIssuer
{
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    private readonly Option<OtpConfiguration> _otpConfiguration = configuration
        .CreateOtpConfiguration()
        .MapFail(error => Error.New("The otp configuration could not be created", error))
        .Map(Some)
        .IfFail(error => LogErrorIO(logger, error.ToString()).Run());
    
    public TimeSpan OtpLifetime { get; } = configuration.GetOtpLifeTime();
    
    public Eff<OneTimePassword> Create(Id<User> id) =>
        from utcNow in timeProvider.UtcNow
        from otpConfiguration in _otpConfiguration.ToEff()
        from otpValue in NonEmptyString.From(GenerateOtpValue(otpConfiguration.Length)).ToEff()
        let expires = utcNow + otpConfiguration.Expiration
        select OneTimePassword.New(id, otpValue, expires);

    private static string GenerateOtpValue(int length)
    {
        Span<byte> randomNumber = stackalloc byte[sizeof(int)];
        RandomNumberGenerator.GetBytes(randomNumber);
        int otp = BitConverter.ToInt32(randomNumber) % (int)Math.Pow(10, length);
        return Math.Abs(otp).ToString().PadLeft(6, '0');
    }
    
    private static IO<Unit> LogErrorIO(ILogger logger, string error) =>
        lift(() => LogError(logger, error));

    [LoggerMessage(Level = LogLevel.Error, Message = "{error}")]
    static partial void LogError(ILogger logger, string error);
}