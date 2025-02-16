using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface IEmailTemplateService
{
    Eff<NonEmptyString> GetOtpEmailBody(NonEmptyString otpValue, TimeSpan expiration);
}