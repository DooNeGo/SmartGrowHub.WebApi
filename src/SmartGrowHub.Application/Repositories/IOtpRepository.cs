using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IOtpRepository : IRepository<OneTimePassword>
{
    OptionT<IO, OneTimePassword> GetByValue(NonEmptyString value, CancellationToken cancellationToken);
}