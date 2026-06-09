using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface IGrowHubRepository : IRepository<GrowHub>
{
    IO<ImmutableList<GrowHub>> GetAllByUserId(Id<User> id);
}