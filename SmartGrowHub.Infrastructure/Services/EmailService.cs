using FluentEmail.Core;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public IEmailService To(EmailAddress emailAddress)
    {
        fluentEmail.To(emailAddress);
        return this;
    }

    public IEmailService Subject(NonEmptyString subject)
    {
        fluentEmail.Subject(subject);
        return this;
    }

    public IEmailService Body(NonEmptyString body, bool isHtml = false)
    {
        fluentEmail.Body(body, isHtml);
        return this;
    }

    public Eff<Unit> Send(CancellationToken cancellationToken) =>
        liftEff(() => fluentEmail.SendAsync(cancellationToken).ToUnit());
}