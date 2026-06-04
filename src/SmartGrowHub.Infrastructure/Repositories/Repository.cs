using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Infrastructure.Data.Extensions;
using SmartGrowHub.Infrastructure.Data.Model;

namespace SmartGrowHub.Infrastructure.Repositories;

internal abstract class Repository<TDomain, TDb> : IRepository<TDomain>
    where TDomain : Entity<TDomain>
    where TDb : class, IContainsId
{
    private readonly DbContext _context;

    protected Repository(DbContext context) => _context = context;

    public OptionT<IO, TDomain> GetById(Id<TDomain> id) => GetByPredicate(db => db.Id == id);

    public IO<Unit> RemoveByIdAndSave(Id<TDomain> id) =>
        IO.liftAsync(env => _context.Set<TDb>()
            .Where(db => db.Id == id)
            .ExecuteDeleteAsync(env.Token)
            .ToUnit());

    public IO<Unit> Add(TDomain domain) =>
        _context.AddIO(ToDb(domain));

    public IO<Unit> AddRange(IEnumerable<TDomain> domains) =>
        _context.AddRangeIO(domains.Select(ToDb));

    public IO<Unit> Remove(TDomain domain) =>
        _context.RemoveIO(ToDb(domain));

    public IO<Unit> RemoveRange(IEnumerable<TDomain> domains) =>
        _context.RemoveRangeIO(domains.Select(ToDb));

    public virtual IO<Unit> Update(TDomain domain) =>
        from option in IO.lift(() => Prelude.Optional(_context.Set<TDb>().Local.FindEntry(domain.Id.Value)))
        let newDb = ToDb(domain)
        from _1 in option.Match(
            Some: entry => IO.lift(() => entry.CurrentValues.SetValues(newDb)),
            None: () => _context.UpdateIO(newDb))
        select _1;

    public IO<Unit> SaveChanges() => _context.SaveChangesIO();

    protected abstract TDb ToDb(TDomain domain);
    
    protected abstract Fin<TDomain> ToDomain(TDb db);
    
    protected abstract IQueryable<TDb> AddIncludes(IQueryable<TDb> query);
    
    protected OptionT<IO, TDomain> GetByPredicate(Expression<Func<TDb, bool>> predicate) =>
        from moduleDb in OptionT.liftIO<IO, TDb>(
            IO.liftAsync(env =>
                AddIncludes(_context.Set<TDb>().Where(predicate))
                    .FirstOrDefaultAsync(env.Token)
                    .Map(Prelude.Optional)))
        from module in ToDomain(moduleDb).ToIO()
        select module;
}