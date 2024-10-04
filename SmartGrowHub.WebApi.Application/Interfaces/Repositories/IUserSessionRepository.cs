using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserSessionRepository
{
    Eff<Unit> Add(UserSession userSession);
    Eff<Unit> Update(UserSession userSession);
    Eff<Unit> RemoveAsync(Id<UserSession> id, CancellationToken cancellationToken);
    Eff<UserSession> GetAsync(RefreshToken token, CancellationToken cancellationToken);
    Eff<Unit> RemoveAllAsync(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> SaveChangesAsync(CancellationToken cancellationToken);
}