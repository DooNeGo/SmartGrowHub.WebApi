using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public sealed class TokenExpirationService
{
    private static readonly Error RefreshTokenExpiredError =
        Error.New("The refresh token has already expired");

    public Fin<Unit> ValidateRefreshToken(RefreshToken refreshToken, DateTime now) =>
        refreshToken.Expires > now ? unit : RefreshTokenExpiredError;
}
