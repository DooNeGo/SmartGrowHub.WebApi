using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserRepository
{
    TryOptionAsync<Fin<User>> GetAsync(UserName userName, CancellationToken cancellationToken);
    TryOptionAsync<Fin<User>> GetAsync(Id<User> id, CancellationToken cancellationToken);
    Try<Unit> Add(User user);
    TryAsync<Unit> SaveChangesAsync(CancellationToken cancellationToken);
}