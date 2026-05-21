using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class EmailService(ISmtpClient smtpClient, IConfiguration configuration)
    : IEmailService
{
    private EmailAddress? _to;
    private NonEmptyString? _subject;
    private NonEmptyString? _body;
    private bool _isHtmlBody;

    public IEmailService To(EmailAddress emailAddress)
    {
        _to = emailAddress;
        return this;
    }

    public IEmailService Subject(NonEmptyString subject)
    {
        _subject = subject;
        return this;
    }

    public IEmailService Body(NonEmptyString body, bool isHtml = false)
    {
        _body = body;
        _isHtmlBody = isHtml;
        return this;
    }

    public IO<Unit> Send(CancellationToken cancellationToken) =>
        IO.liftAsync(async () =>
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(configuration["Email:SenderName"], configuration["Email:SenderEmail"]));
            message.To.Add(new MailboxAddress(string.Empty, _to));
            message.Subject = _subject;
            message.Body = new TextPart(_isHtmlBody ? "html" : "plain") { Text = _body };
            
            await smtpClient.ConnectAsync(
                configuration["Email:Host"],
                configuration.GetValue<int>("Email:Port"),
                configuration.GetValue<bool>("Email:UseSsl"),
                cancellationToken);
            
            await smtpClient.AuthenticateAsync(
                configuration["Email:Username"],
                configuration["Email:Password"],
                cancellationToken);

            await smtpClient.SendAsync(message, cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);
            
            return Unit.Default;
        });
}