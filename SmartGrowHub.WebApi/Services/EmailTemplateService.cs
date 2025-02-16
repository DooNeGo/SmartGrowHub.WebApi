using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.WebApi.Services;

internal sealed class EmailTemplateService(IWebHostEnvironment environment) : IEmailTemplateService
{
    public Eff<NonEmptyString> GetOtpEmailBody(NonEmptyString otpValue, TimeSpan expiration) =>
        GetEmailBody("OtpEmailTemplate.html",
        [
            ("OTP", otpValue),
            ("Expiration", expiration.Minutes.ToString())
        ]);

    private Eff<NonEmptyString> GetEmailBody(string templateName, (string, string)[] placeholders) =>
        from template in GetTemplate(templateName)
        let otpEmailBody = ReplacePlaceholders(template, placeholders)
        from result in NonEmptyString.From(otpEmailBody).ToEff()
        select result;

    private Eff<string> GetTemplate(string templateName) =>
        liftEff(() => File.ReadAllTextAsync(Path.Combine(environment.WebRootPath, "templates", templateName)));

    private static string ReplacePlaceholders(string template, (string, string)[] placeholders)
    {
        foreach ((string key, string value) in placeholders.AsSpan())
        {
            template = template.Replace($"{{{key}}}", value);
        }
        
        return template;
    }
}