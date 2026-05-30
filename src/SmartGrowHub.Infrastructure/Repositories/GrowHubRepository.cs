using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class GrowHubRepository : Repository<GrowHub, GrowHubDb>, IGrowHubRepository
{
    private readonly ApplicationContext _context;

    public GrowHubRepository(ApplicationContext context) : base(context) => _context = context;

    public IO<Iterable<GrowHub>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken) =>
        from list in IO.liftAsync(() =>
            AddIncludes(_context.GrowHubs.Where(x => x.UserId == id.Value))
                .ToListAsync(cancellationToken))
        from domains in list
            .AsIterable()
            .Traverse(ToDomain)
            .As().ToIO()
        select domains;

    protected override GrowHubDb ToDb(GrowHub domain) => domain.ToDb();

    protected override Fin<GrowHub> ToDomain(GrowHubDb db) => db.ToDomain();

    protected override IQueryable<GrowHubDb> AddIncludes(IQueryable<GrowHubDb> query) =>
        query.Include(x => x.Modules).ThenInclude(x => x.Program);
}