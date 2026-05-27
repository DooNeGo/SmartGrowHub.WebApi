using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface IEmailService
{
    IO<Unit> Send(EmailAddress to, NonEmptyString subject, NonEmptyString body, bool isHtmlBody,
        CancellationToken cancellationToken);
}