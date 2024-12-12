using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Services;

public interface ISmsService
{
    public Eff<Unit> Send(PhoneNumber phoneNumber, NonEmptyString payload, CancellationToken cancellationToken);
}