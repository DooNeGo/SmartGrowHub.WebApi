using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IGrowHubRepository : IRepository<GrowHub>
{
    IO<Iterable<GrowHub>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken);
}