using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Repositories;

public interface IRepository<TDomain> where TDomain : Entity<TDomain>
{
    OptionT<IO, TDomain> GetById(Id<TDomain> id, CancellationToken cancellationToken);
    IO<Unit> Add(TDomain domain, CancellationToken cancellationToken);
    IO<Unit> Remove(TDomain domain, CancellationToken cancellationToken);
    IO<Unit> RemoveById(Id<TDomain> id, CancellationToken cancellationToken);
    IO<Unit> Update(TDomain domain, CancellationToken cancellationToken);
}