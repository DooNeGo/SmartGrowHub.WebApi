using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserService
{
    Eff<Unit> AddNewUser(UserName userName, PlainTextPassword password, EmailAddress email,
        NonEmptyString displayName, CancellationToken cancellationToken);
    Eff<UserSession> AddNewSessionToUser(User user, CancellationToken cancellationToken);
    Eff<AuthTokens> RefreshTokens(RefreshToken oldToken, CancellationToken cancellationToken);
}