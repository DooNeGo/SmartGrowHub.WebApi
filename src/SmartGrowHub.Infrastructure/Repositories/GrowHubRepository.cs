using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class GrowHubRepository(ApplicationContext context) : IGrowHubRepository
{
    public IO<Iterable<GrowHub>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken) =>
        from list in IO.liftAsync(() => context.GrowHubs
            .Where(x => x.UserId == id.Value)
            .Include(x => x.Modules)
            .ThenInclude(x => x.Program)
            .ToListAsync(cancellationToken))
        from domains in list
            .AsIterable()
            .Traverse(x => x.ToDomain())
            .As().ToIO()
        select domains;
    
    public IO<Unit> Add(GrowHub growHub, CancellationToken cancellationToken) =>
        Add(growHub) >> SaveChanges(cancellationToken);
    
    public IO<Unit> Remove(GrowHub growHub, CancellationToken cancellationToken) =>
        Remove(growHub) >> SaveChanges(cancellationToken);

    public IO<Unit> Update(GrowHub growHub, CancellationToken cancellationToken) =>
        Update(growHub) >> SaveChanges(cancellationToken);

    private IO<Unit> Add(GrowHub growHub) =>
        IO.lift(() => context.GrowHubs.Add(growHub.ToDb()))
            .ToUnit();

    private IO<Unit> Remove(GrowHub growHub) =>
        IO.lift(() => context.GrowHubs.Remove(growHub.ToDb()))
            .ToUnit();

    private IO<Unit> Update(GrowHub growHub) =>
        IO.lift(() => context.GrowHubs.Update(growHub.ToDb()))
            .ToUnit();

    private IO<Unit> SaveChanges(CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.SaveChangesAsync(cancellationToken))
            .ToUnit();
}