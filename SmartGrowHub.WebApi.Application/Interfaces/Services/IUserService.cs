using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserService
{
    Eff<Unit> AddAsync(User user, CancellationToken cancellationToken);
    Eff<User> GetAsync(UserName userName, CancellationToken cancellationToken);
    Eff<User> GetAsync(Id<User> id, CancellationToken cancellationToken);
}