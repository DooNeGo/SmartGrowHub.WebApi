using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

internal static class AuthTokensExtensions
{
    public static AuthTokensDto ToDto(this AuthTokens tokens) =>
        new(tokens.AccessToken.To(), tokens.RefreshToken.Value);
}