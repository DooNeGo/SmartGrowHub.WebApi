using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IOtpRepository
{
    Eff<Unit> Add(OneTimePassword oneTimePassword, CancellationToken cancellationToken);
    Eff<Unit> Remove(Id<OneTimePassword> id, CancellationToken cancellationToken);
    Eff<ImmutableArray<OneTimePassword>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
    Eff<OneTimePassword> GetByValue(NonEmptyString value, CancellationToken cancellationToken);
}