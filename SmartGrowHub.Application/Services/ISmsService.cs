using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface ISmsService
{
    IO<Unit> Send(PhoneNumber phoneNumber, NonEmptyString payload, CancellationToken cancellationToken);
}