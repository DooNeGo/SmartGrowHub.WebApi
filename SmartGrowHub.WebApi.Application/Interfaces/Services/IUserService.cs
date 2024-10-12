using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserService
{
    Eff<User> GetByUserName(UserName userName, CancellationToken cancellationToken);
    Eff<Unit> AddNewUser(User user, CancellationToken cancellationToken);
    Eff<User> RemoveSession(Id<UserSession> sessionId, CancellationToken cancellationToken);
    Eff<(User User, UserSession Session)> AddNewSessionToUser(User user, CancellationToken cancellationToken);
    Eff<AuthTokens> RefreshTokens(RefreshToken oldToken, CancellationToken cancellationToken);
}