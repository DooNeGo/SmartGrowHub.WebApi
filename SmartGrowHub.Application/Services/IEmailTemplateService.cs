using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface IEmailTemplateService
{
    Eff<NonEmptyString> GetOtpEmailBody(int otpValue, TimeSpan expiration);
}