using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserSessionService
{
    Eff<UserSession> CreateAsync(User user, CancellationToken cancellationToken);
    Eff<AuthTokens> RefreshTokensAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Eff<Unit> RemoveAsync(Id<UserSession> id, CancellationToken cancellationToken);
}