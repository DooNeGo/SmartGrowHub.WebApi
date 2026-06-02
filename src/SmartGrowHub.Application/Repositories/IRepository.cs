using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.Repositories;

public static class RepositoryExtensions
{
    extension<TDomain>(IRepository<TDomain> repository) where TDomain : Entity<TDomain>
    {
        public IO<Unit> AddAndSave(TDomain domain) =>
            repository.Add(domain) >> repository.SaveChanges();

        public IO<Unit> AddRangeAndSave(IEnumerable<TDomain> domains) =>
            repository.AddRange(domains) >> repository.SaveChanges();

        public IO<Unit> RemoveAndSave(TDomain domain) =>
            repository.Remove(domain) >> repository.SaveChanges();

        public IO<Unit> RemoveRangeAndSave(IEnumerable<TDomain> domains) =>
            repository.RemoveRange(domains) >> repository.SaveChanges();
        
        public IO<Unit> UpdateAndSave(TDomain domain) =>
            repository.Update(domain) >> repository.SaveChanges();
    }
}

public interface IRepository<TDomain> where TDomain : Entity<TDomain>
{
    OptionT<IO, TDomain> GetById(Id<TDomain> id);
    IO<Unit> RemoveByIdAndSave(Id<TDomain> id);
    IO<Unit> Add(TDomain domain);
    IO<Unit> AddRange(IEnumerable<TDomain> domains);
    IO<Unit> Remove(TDomain domain);
    IO<Unit> RemoveRange(IEnumerable<TDomain> domains);
    IO<Unit> Update(TDomain domain);
    IO<Unit> SaveChanges();
}