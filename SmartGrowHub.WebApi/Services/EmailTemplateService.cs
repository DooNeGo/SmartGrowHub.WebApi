using System.Text;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Application.UseCases.Auth;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.WebApi.Services;

internal sealed class EmailTemplateService(
    IWebHostEnvironment environment,
    IFileService fileService)
    : IEmailTemplateService
{
    public IO<NonEmptyString> GetOtpEmailBody(NonEmptyString otpValue, TimeSpan expiration,
        CancellationToken cancellationToken) =>
        GetEmailBody("OtpEmailTemplate.html",
        [
            ("OTP", otpValue),
            ("Expiration", expiration.Minutes.ToString())
        ], cancellationToken);

    private IO<NonEmptyString> GetEmailBody(string templateName, (string, string)[] placeholders,
        CancellationToken cancellationToken) =>
        from template in GetTemplate(templateName, cancellationToken)
        let otpEmailBody = ReplacePlaceholders(template, placeholders)
        from result in NonEmptyString.From(otpEmailBody).ToIO()
        select result;

    private IO<string> GetTemplate(string templateName, CancellationToken cancellationToken)
    {
        string path = Path.Combine(environment.WebRootPath, "templates", templateName);
        return fileService.ReadAllTextAsync(path, Encoding.Default, cancellationToken);
    }

    private static string ReplacePlaceholders(string template, (string, string)[] placeholders)
    {
        foreach ((string key, string value) in placeholders.AsSpan())
        {
            template = template.Replace($"{{{key}}}", value);
        }

        return template;
    }
}