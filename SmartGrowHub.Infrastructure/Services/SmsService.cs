using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using Vonage.Messaging;
using Vonage.Request;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed partial class SmsService(
    IConfiguration configuration,
    ILogger<SmsService> logger)
    : ISmsService
{
    private readonly Option<SmsClient> _smsClient = configuration
        .CreateSmsCredentials()
        .MapFail(error => Error.New("Failed to create sms service", error))
        .Map(credentials => Some(new SmsClient(credentials)))
        .IfFail(error => LogErrorIO(logger, error.ToString()).Run());
    
    public Eff<Unit> Send(PhoneNumber phoneNumber, NonEmptyString payload, CancellationToken cancellationToken) =>
        from smsClient in _smsClient.ToEff()
        let request = new SendSmsRequest
        {
            To = phoneNumber,
            From = "Test",
            Text = payload
        }
        from _ in liftEff(() => smsClient.SendAnSmsAsync(request).WaitAsync(cancellationToken))
        select unit;

    private static IO<Unit> LogErrorIO(ILogger logger, string error) =>
        lift(() => LogError(logger, error));

    [LoggerMessage(Level = LogLevel.Error, Message = "{error}")]
    static partial void LogError(ILogger logger, string error);
}

internal static class SmsCredentialsCreation
{
    public static Fin<Credentials> CreateSmsCredentials(this IConfiguration configuration) =>
        from apiKey in NonEmptyString.From(configuration["Vonage:ApiKey"]!)
        from apiSecret in NonEmptyString.From(configuration["Vonage:ApiSecret"]!)
        select Credentials.FromApiKeyAndSecret(apiKey, apiSecret);
}