using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class EmailService(ISmtpClient smtpClient, IConfiguration configuration)
    : IEmailService
{
    public IO<Unit> Send(EmailAddress to, NonEmptyString subject, NonEmptyString body, bool isHtmlBody,
        CancellationToken cancellationToken) =>
        IO.liftAsync(async () =>
        {
            IConfigurationSection section = configuration.GetRequiredSection("Smtp");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(section["Sender:Name"], section["Sender:Email"]));
            message.To.Add(new MailboxAddress(string.Empty, to));
            message.Subject = subject;
            message.Body = new TextPart(isHtmlBody ? "html" : "plain") { Text = body };

            await smtpClient.ConnectAsync(
                section["Host"],
                section.GetValue<int>("Port"),
                section.GetValue<bool>("UseSsl"),
                cancellationToken);

            await smtpClient.AuthenticateAsync(section["Username"], section["Password"], cancellationToken);
            await smtpClient.SendAsync(message, cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);

            return Unit.Default;
        });
}