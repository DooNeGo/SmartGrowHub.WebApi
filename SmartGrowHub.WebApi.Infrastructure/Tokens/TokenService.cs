using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using SmartGrowHub.WebApi.Infrastructure.Tokens;
using System.Security.Claims;

namespace SmartGrowHub.WebApi.Infrastructure;

internal sealed partial class TokenService(
    IConfiguration configuration,
    ILogger<TokenService> logger,
    ITimeProvider timeProvider)
    : ITokenService
{
    private readonly JsonWebTokenHandler _tokenHandler = new();

    private readonly Option<TokensConfiguration> _tokensConfiguration =
        configuration.CreateTokensConfiguration()
            .Map(result => Some(result))
            .IfFail(error => LogErrorIO(logger, error.Message)
                .Map(_ => None).Run());

    public Eff<AuthTokens> CreateTokens(User user) =>
        from utcNow in timeProvider.GetUtcNow()
        from configurations in _tokensConfiguration.ToEff()
        from accessToken in CreateAccessToken(user, configurations.AccessTokenConfiguration, utcNow).ToEff()
        let refreshtoken = CreateRefreshToken(configurations.RefreshTokenConfiguration, utcNow)
        select new AuthTokens(accessToken, refreshtoken);

    private static RefreshToken CreateRefreshToken(RefreshTokenConfiguration configuration, DateTime now) =>
        new(Ulid.NewUlid(), now + configuration.Expiration);

    private Fin<AccessToken> CreateAccessToken(User user, AccessTokenConfiguration configuration, DateTime now) =>
        AccessToken.From(_tokenHandler.CreateToken(new SecurityTokenDescriptor()
        {
            Issuer = configuration.Issuer,
            Audience = configuration.Audience,
            Expires = now + configuration.AccessTokenExpiration,
            SigningCredentials = configuration.SigningCredentials,
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
        }));

    private static IO<Unit> LogErrorIO(ILogger logger, string error) =>
        lift(() => LogError(logger, error));

    [LoggerMessage(Level = LogLevel.Error, Message = "{error}")]
    static partial void LogError(ILogger logger, string error);
}
