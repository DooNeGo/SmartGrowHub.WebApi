using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface IEmailTemplateService
{
    IO<NonEmptyString> GetOtpEmailBody(NonEmptyString otpValue, TimeSpan expiration,
        CancellationToken cancellationToken);
}