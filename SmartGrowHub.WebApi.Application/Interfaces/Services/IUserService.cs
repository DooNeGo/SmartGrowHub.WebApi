using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserService
{
    Eff<Unit> AddNewUser(User user, CancellationToken cancellationToken);
    Eff<UserSession> AddNewSessionToUser(User user, CancellationToken cancellationToken);
    Eff<AuthTokens> RefreshTokens(RefreshToken oldToken, CancellationToken cancellationToken);
}