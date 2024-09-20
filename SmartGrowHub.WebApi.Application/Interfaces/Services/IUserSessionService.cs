using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserSessionService
{
    EitherAsync<Exception, UserSession> CreateAsync(User user, CancellationToken cancellationToken);
    EitherAsync<Exception, RefreshTokensResponse> RefreshTokensAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}