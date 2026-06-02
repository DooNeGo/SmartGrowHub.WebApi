using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class EmailService(ISmtpClient smtpClient, IConfiguration configuration)
    : IEmailService
{
    public IO<Unit> Send(EmailAddress to, NonEmptyString subject, NonEmptyString body, bool isHtmlBody) =>
        IO.liftAsync(async env =>
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
                env.Token);

            await smtpClient.AuthenticateAsync(section["Username"], section["Password"], env.Token);
            await smtpClient.SendAsync(message, env.Token);
            await smtpClient.DisconnectAsync(true, env.Token);

            return Unit.Default;
        });
}