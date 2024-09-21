using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Eff<User> GetAsync(UserName userName, CancellationToken cancellationToken);
    Eff<User> GetAsync(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> Add(User user);
    Eff<Unit> SaveChangesAsync(CancellationToken cancellationToken);
}