using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IOtpRepository
{
    IO<Unit> Add(OneTimePassword oneTimePassword, CancellationToken cancellationToken);
    IO<Unit> Remove(Id<OneTimePassword> id, CancellationToken cancellationToken);
    IO<ImmutableArray<OneTimePassword>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
    OptionT<IO, OneTimePassword> GetByValue(NonEmptyString value, CancellationToken cancellationToken);
}