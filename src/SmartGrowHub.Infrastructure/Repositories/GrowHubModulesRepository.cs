using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class GrowHubModulesRepository : Repository<GrowHubModule, GrowHubModuleDb>, IGrowHubModulesRepository
{
    public GrowHubModulesRepository(ApplicationContext context) : base(context) { }

    protected override GrowHubModuleDb ToDb(GrowHubModule domain) => domain.ToDb();

    protected override Fin<GrowHubModule> ToDomain(GrowHubModuleDb db) => db.ToDomain();
    
    protected override IQueryable<GrowHubModuleDb> AddIncludes(IQueryable<GrowHubModuleDb> query) =>
        query.Include(x => x.Schedule);

    public OptionT<IO, GrowHubModule> GetByScheduleId(Id<ModuleSchedule> id, CancellationToken cancellationToken) =>
        GetByPredicate(module => module.Schedule.Id == id, cancellationToken);
}