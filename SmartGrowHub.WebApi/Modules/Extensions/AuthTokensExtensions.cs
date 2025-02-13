using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.WebApi.Modules.Extensions;

public static class AuthTokensExtensions
{
    public static AuthTokensDto ToDto(this AuthTokens tokens) =>
        new(tokens.AccessToken.To(), tokens.RefreshToken.Ulid.ToString());
}