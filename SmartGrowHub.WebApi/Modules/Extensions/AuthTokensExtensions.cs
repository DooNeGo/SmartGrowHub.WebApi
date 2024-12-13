using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.WebApi.Modules.Extensions;

public static class AuthTokensExtensions
{
    public static AuthTokensDto ToDto(this AuthTokens tokens) =>
        new(tokens.AccessToken.To(), ToDto(tokens.RefreshToken));

    public static RefreshTokenDto ToDto(this RefreshToken token) =>
        new(token.Ulid.ToString(), token.Expires);
}