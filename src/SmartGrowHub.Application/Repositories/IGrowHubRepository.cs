using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.GrowHub;

namespace SmartGrowHub.Application.Repositories;

public interface IGrowHubRepository
{
    IO<Iterable<GrowHub>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
    IO<Unit> Add(GrowHub growHub, CancellationToken cancellationToken);
    IO<Unit> Remove(GrowHub growHub, CancellationToken cancellationToken);
    IO<Unit> Update(GrowHub growHub, CancellationToken cancellationToken);
}