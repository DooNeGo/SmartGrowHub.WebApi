using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Tokens;

internal sealed partial class TokensIssuer(
    IConfiguration configuration,
    ILogger<TokensIssuer> logger,
    ITimeProvider timeProvider)
    : ITokensIssuer
{
    private readonly JsonWebTokenHandler _tokenHandler = new();

    private readonly Option<TokensConfiguration> _tokensConfiguration = configuration
        .CreateTokensConfiguration()
        .MapFail(error => Error.New("The jwt tokens configuration could not be created", error))
        .Map(Some)
        .IfFail(error => LogErrorIO(logger, error.ToString()).Run());

    public Eff<AuthTokens> CreateTokens(User user) =>
        from utcNow in timeProvider.UtcNow
        from configurations in _tokensConfiguration.ToEff()
        from accessToken in CreateAccessToken(user, configurations.AccessTokenConfiguration, utcNow).ToEff()
        let refreshToken = CreateRefreshToken(configurations.RefreshTokenConfiguration, utcNow)
        select new AuthTokens(accessToken, refreshToken);

    private static RefreshToken CreateRefreshToken(RefreshTokenConfiguration configuration, DateTime now) =>
        RefreshToken.New(now + configuration.Expiration);

    private Fin<AccessToken> CreateAccessToken(User user, AccessTokenConfiguration configuration, DateTime now) =>
        AccessToken.From(_tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = configuration.Issuer,
            Audience = configuration.Audience,
            Expires = now + configuration.AccessTokenExpiration,
            SigningCredentials = configuration.SigningCredentials,
            Subject = CreateClaimsIdentity(user)
        }));

    public static ClaimsIdentity CreateClaimsIdentity(User user)
    {
        var claimsIdentity = new ClaimsIdentity();
        
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        
        user.PhoneNumber.IfSome(phoneNumber =>
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.PhoneNumber, phoneNumber)));
        
        user.Email.IfSome(email =>
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, email)));
        
        return claimsIdentity;
    }
    
    private static IO<Unit> LogErrorIO(ILogger logger, string error) =>
        lift(() => LogError(logger, error));

    [LoggerMessage(Level = LogLevel.Error, Message = "{error}")]
    static partial void LogError(ILogger logger, string error);
}
