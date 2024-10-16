using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public sealed class TokenExpirationService
{
    public Fin<Unit> ValidateRefreshToken(RefreshToken refreshToken, DateTime now) =>
        refreshToken.Expires > now ? unit : DomainErrors.RefreshTokenExpiredError;
}
