using System.Text;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.AspNetCore.Services;

internal sealed class EmailTemplateService(
    IWebHostEnvironment environment,
    IFileService fileService)
    : IEmailTemplateService
{
    public IO<NonEmptyString> GetOtpEmailBody(NonEmptyString otpValue, TimeSpan expiration) =>
        GetEmailBody("OtpEmailTemplate.html", [("OTP", otpValue), ("Expiration", expiration.Minutes.ToString())]);

    private IO<NonEmptyString> GetEmailBody(string templateName, (string, string)[] placeholders) =>
        from template in GetTemplate(templateName)
        let otpEmailBody = ReplacePlaceholders(template, placeholders)
        from result in NonEmptyString.From(otpEmailBody).ToIO()
        select result;

    private IO<string> GetTemplate(string templateName)
    {
        string path = Path.Combine(environment.WebRootPath, "templates", templateName);
        return fileService.ReadAllText(path, Encoding.Default);
    }

    private static string ReplacePlaceholders(string template, (string, string)[] placeholders)
    {
        var stringBuilder = new StringBuilder(template);
        
        foreach ((string key, string value) in placeholders.AsSpan())
        {
            stringBuilder.Replace($"{{{key}}}", value);
        }

        return stringBuilder.ToString();
    }
}