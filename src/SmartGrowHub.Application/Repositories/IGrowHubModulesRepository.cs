using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub;

namespace SmartGrowHub.Application.Repositories;

public interface IGrowHubModulesRepository
{
    OptionT<IO, GrowHubModule> GetById(Id<GrowHubModule> id, CancellationToken cancellationToken);
    IO<Unit> Add(GrowHubModule growHub, CancellationToken cancellationToken);
    IO<Unit> Remove(GrowHubModule growHub, CancellationToken cancellationToken);
    IO<Unit> Update(GrowHubModule growHub, CancellationToken cancellationToken);
}