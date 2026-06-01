using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.Repositories;

public interface IGrowHubModulesRepository : IRepository<GrowHubModule>
{
    OptionT<IO, GrowHubModule> GetByScheduleId(Id<ModuleSchedule> id, CancellationToken cancellationToken);
}