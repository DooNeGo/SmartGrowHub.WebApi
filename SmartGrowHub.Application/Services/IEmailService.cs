using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface IEmailService
{
    IEmailService To(EmailAddress emailAddress);
    IEmailService Subject(NonEmptyString subject);
    IEmailService Body(NonEmptyString body, bool isHtml = false);
    IO<Unit> Send(CancellationToken cancellationToken);
}