using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Eff<User> GetByUserName(UserName userName, CancellationToken cancellationToken);
    Eff<User> GetById(Id<User> id, CancellationToken cancellationToken);
    Eff<User> GetBySessionId(Id<UserSession> id, CancellationToken cancellationToken);
    Eff<User> GetByRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    Eff<Unit> Add(User user, CancellationToken cancellationToken);
    Eff<Unit> Remove(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> Update(User user, CancellationToken cancellationToken);
}