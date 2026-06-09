using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class GrowHubRepository : Repository<GrowHub, GrowHubDb>, IGrowHubRepository
{
    public GrowHubRepository(ApplicationContext context) : base(context) { }

    public IO<ImmutableList<GrowHub>> GetAllByUserId(Id<User> id) =>
        GetAllByPredicate(hub => hub.UserId == id);

    protected override GrowHubDb ToDb(GrowHub domain) => domain.ToDb();

    protected override Fin<GrowHub> ToDomain(GrowHubDb db) => db.ToDomain();

    protected override IQueryable<GrowHubDb> AddIncludes(IQueryable<GrowHubDb> query) =>
        query
            .Include(x => x.Modules)
            .ThenInclude(x => x.Schedule)
            .ThenInclude(x => x.Units);
}