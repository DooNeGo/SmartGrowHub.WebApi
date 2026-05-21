using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.Domain.Common;
using System.Text;

namespace SmartGrowHub.Infrastructure.Tokens;

public static class TokensConfigurationCreation
{
    public static Fin<TokensConfiguration> CreateTokensConfiguration(this IConfiguration configuration) =>
        from accessTokenConfiguration in configuration.CreateAccessTokenConfiguration()
        from refreshTokenConfiguration in configuration.CreateRefreshTokenConfiguration()
        select new TokensConfiguration(accessTokenConfiguration, refreshTokenConfiguration);

    public static Fin<AccessTokenConfiguration> CreateAccessTokenConfiguration(this IConfiguration configuration) =>
        from issuer in NonEmptyString.From(configuration["Jwt:Issuer"]!)
        from audience in NonEmptyString.From(configuration["Jwt:Audience"]!)
        from accessTokenExpirationInMinutes in Prelude.parseInt(configuration["Jwt:AccessTokenExpirationInMinutes"]!).ToFin()
        from secret in NonEmptyString.From(configuration["Jwt:Secret"]!)
        let signingCredentials = CreateSigningCredentials(secret)
        let accessTokenExpiration = TimeSpan.FromMinutes(accessTokenExpirationInMinutes)
        select new AccessTokenConfiguration(issuer, audience, signingCredentials, accessTokenExpiration);

    public static Fin<RefreshTokenConfiguration> CreateRefreshTokenConfiguration(this IConfiguration configuration) =>
        from refreshTokenExpirationInDays in Prelude.parseInt(configuration["Jwt:RefreshTokenExpirationInDays"]!).ToFin()
        let refreshTokenExpiration = TimeSpan.FromDays(refreshTokenExpirationInDays)
        select new RefreshTokenConfiguration(refreshTokenExpiration);

    private static SigningCredentials CreateSigningCredentials(string key) =>
        new(CreateSymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

    private static SymmetricSecurityKey CreateSymmetricSecurityKey(string key) =>
        new(Encoding.UTF8.GetBytes(key));
}
