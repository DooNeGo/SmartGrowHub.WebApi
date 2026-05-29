using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model.GrowHub;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class GrowHubModulesRepository(ApplicationContext context) : IGrowHubModulesRepository
{
    public OptionT<IO, GrowHubModule> GetById(Id<GrowHubModule> id, CancellationToken cancellationToken) =>
        from moduleDb in OptionT.liftIO<IO, GrowHubModuleDb>(
            IO.liftAsync(() => context.Modules
                .Where(x => x.Id == id.Value)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(cancellationToken)
                .Map(Prelude.Optional)))
        from module in moduleDb.ToDomain().ToIO()
        select module;
    
    public IO<Unit> Add(GrowHubModule growHub, CancellationToken cancellationToken) =>
        Add(growHub) >> SaveChanges(cancellationToken);
    
    public IO<Unit> Remove(GrowHubModule growHub, CancellationToken cancellationToken) =>
        Remove(growHub) >> SaveChanges(cancellationToken);

    public IO<Unit> Update(GrowHubModule growHub, CancellationToken cancellationToken) =>
        Update(growHub) >> SaveChanges(cancellationToken);

    private IO<Unit> Add(GrowHubModule growHub) =>
        IO.lift(() => context.Modules.Add(growHub.ToDb()))
            .ToUnit();

    private IO<Unit> Remove(GrowHubModule growHub) =>
        IO.lift(() => context.Modules.Remove(growHub.ToDb()))
            .ToUnit();

    private IO<Unit> Update(GrowHubModule growHub) =>
        IO.lift(() => context.Modules.Update(growHub.ToDb()))
            .ToUnit();

    private IO<Unit> SaveChanges(CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.SaveChangesAsync(cancellationToken))
            .ToUnit();
}