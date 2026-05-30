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

    public OptionT<IO, TDomain> GetById(Id<TDomain> id, CancellationToken cancellationToken) =>
        GetByPredicate(db => db.Id == id, cancellationToken);

    public IO<Unit> Add(TDomain domain, CancellationToken cancellationToken) =>
        _context.AddIO(ToDb(domain)) >> _context.SaveChangesIO(cancellationToken);
    
    public IO<Unit> Remove(TDomain domain, CancellationToken cancellationToken) =>
        _context.RemoveIO(ToDb(domain)) >> _context.SaveChangesIO(cancellationToken);

    public IO<Unit> RemoveById(Id<TDomain> id, CancellationToken cancellationToken) =>
        IO.liftAsync(() => _context.Set<TDb>()
            .Where(db => db.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public IO<Unit> Update(TDomain domain, CancellationToken cancellationToken) =>
        _context.UpdateIO(ToDb(domain)) >> _context.SaveChangesIO(cancellationToken);

    protected abstract TDb ToDb(TDomain domain);
    
    protected abstract Fin<TDomain> ToDomain(TDb db);
    
    protected abstract IQueryable<TDb> AddIncludes(IQueryable<TDb> query);
    
    protected OptionT<IO, TDomain> GetByPredicate(Expression<Func<TDb, bool>> predicate,
        CancellationToken cancellationToken) =>
        from moduleDb in OptionT.liftIO<IO, TDb>(
            IO.liftAsync(() =>
                AddIncludes(_context.Set<TDb>().Where(predicate))
                    .FirstOrDefaultAsync(cancellationToken)
                    .Map(Prelude.Optional)))
        from module in ToDomain(moduleDb).ToIO()
        select module;
}