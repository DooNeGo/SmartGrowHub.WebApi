using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface ISmsService
{
    Eff<Unit> Send(PhoneNumber phoneNumber, NonEmptyString payload, CancellationToken cancellationToken);
}