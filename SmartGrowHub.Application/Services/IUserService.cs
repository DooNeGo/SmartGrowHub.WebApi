using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Services;

public interface IUserService
{
    Eff<UserSession> AddNewSessionToUser(User user, CancellationToken cancellationToken);
    Eff<AuthTokens> RefreshTokens(RefreshToken oldToken, CancellationToken cancellationToken);
}