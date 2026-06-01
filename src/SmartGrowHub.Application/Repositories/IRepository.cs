using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Repositories;

public static class RepositoryExtensions
{
    extension<TDomain>(IRepository<TDomain> repository) where TDomain : Entity<TDomain>
    {
        public IO<Unit> AddAndSave(TDomain domain, CancellationToken cancellationToken) =>
            repository.Add(domain) >> repository.SaveChanges(cancellationToken);

        public IO<Unit> AddRangeAndSave(IEnumerable<TDomain> domains, CancellationToken cancellationToken) =>
            repository.AddRange(domains) >> repository.SaveChanges(cancellationToken);

        public IO<Unit> RemoveAndSave(TDomain domain, CancellationToken cancellationToken) =>
            repository.Remove(domain) >> repository.SaveChanges(cancellationToken);

        public IO<Unit> RemoveRangeAndSave(IEnumerable<TDomain> domains, CancellationToken cancellationToken) =>
            repository.RemoveRange(domains) >> repository.SaveChanges(cancellationToken);
        
        public IO<Unit> UpdateAndSave(TDomain domain, CancellationToken cancellationToken) =>
            repository.Update(domain) >> repository.SaveChanges(cancellationToken);
    }
}

public interface IRepository<TDomain> where TDomain : Entity<TDomain>
{
    OptionT<IO, TDomain> GetById(Id<TDomain> id, CancellationToken cancellationToken);
    IO<Unit> RemoveByIdAndSave(Id<TDomain> id, CancellationToken cancellationToken);
    IO<Unit> Add(TDomain domain);
    IO<Unit> AddRange(IEnumerable<TDomain> domains);
    IO<Unit> Remove(TDomain domain);
    IO<Unit> RemoveRange(IEnumerable<TDomain> domains);
    IO<Unit> Update(TDomain domain);
    IO<Unit> SaveChanges(CancellationToken cancellationToken);
}