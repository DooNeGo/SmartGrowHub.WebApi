using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface IEmailTemplateService
{
    Fin<NonEmptyString> GetOtpEmailBody(int otpValue, TimeSpan expiration);
}