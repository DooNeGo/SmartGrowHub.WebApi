using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserSessionRepository
{
    Eff<Unit> Add(UserSession userSession);
    Eff<UserSession> GetAsync(RefreshToken token, CancellationToken cancellationToken);
    Eff<Unit> RemoveAllAsync(Id<User> id, CancellationToken cancellationToken);
    Eff<Unit> SaveChangesAsync(CancellationToken cancellationToken);
    Eff<Unit> UpdateAsync(UserSession userSession, CancellationToken cancellationToken);
}