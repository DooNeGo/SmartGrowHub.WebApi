using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.WebApi.Services;

internal sealed class EmailTemplateService(IWebHostEnvironment environment) : IEmailTemplateService
{
    public Fin<NonEmptyString> GetOtpEmailBody(int otpValue, TimeSpan expiration) =>
        GetEmailBody("OtpEmailTemplate.html", new Dictionary<string, string>
        {
            { "OTP", otpValue.ToString() },
            { "Expiration", expiration.Minutes.ToString() }
        });

    private Fin<NonEmptyString> GetEmailBody(string templateName, Dictionary<string, string> placeholders)
    {
        string template = File.ReadAllText(Path.Combine(environment.WebRootPath, "templates", templateName));

        foreach (KeyValuePair<string, string> placeholder in placeholders)
        {
            template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
        }

        return NonEmptyString.From(template);
    }
}