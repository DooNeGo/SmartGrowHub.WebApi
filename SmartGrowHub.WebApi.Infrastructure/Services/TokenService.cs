using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using System.Security.Claims;
using System.Text;

namespace SmartGrowHub.WebApi.Infrastructure;

public sealed class KeyNotFoundInConfigurationException(string key)
    : Exception($"{key} was not found in configuration");

internal sealed partial class TokenService : ITokenService
{
    private readonly JsonWebTokenHandler _tokenHandler = new();

    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationInMinutes;
    private readonly int _refreshTokenExpirationInDays;

    private readonly SigningCredentials _signingCredentials;

    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        try
        {
            _issuer = GetValueOrException<string>(configuration, "Jwt:Issuer");
            _audience = GetValueOrException<string>(configuration, "Jwt:Audience");
            _accessTokenExpirationInMinutes = GetValueOrException<int>(configuration, "Jwt:AccessTokenExpirationInMinutes");
            _refreshTokenExpirationInDays = GetValueOrException<int>(configuration, "Jwt:RefreshTokenExpirationInDays");
            string secret = GetValueOrException<string>(configuration, "Jwt:Secret");
            _signingCredentials = CreateSigningCredentials(secret);
        }
        catch (Exception exception)
        {
            LogError(logger, exception.Message);
            throw;
        }
    }

    public AuthTokens CreateTokens(User user) =>
        new(CreateAccessToken(user), CreateRefreshToken(user));

    private AccessToken CreateAccessToken(User user) =>
        new(CreateToken(user, DateTime.UtcNow.AddMinutes(_accessTokenExpirationInMinutes)));

    private RefreshToken CreateRefreshToken(User user) =>
        new(CreateToken(user, DateTime.UtcNow.AddDays(_refreshTokenExpirationInDays)));

    private NonEmptyString CreateToken(User user, DateTime expires) =>
        (NonEmptyString)_tokenHandler.CreateToken(
            CreateTokenDescriptor(_issuer, _audience, expires, _signingCredentials, user));

    private static SecurityTokenDescriptor CreateTokenDescriptor(
        string issuer, string audience, DateTime expires,
        SigningCredentials signingCredentials, User user) => new()
        {
            Issuer = issuer,
            Audience = audience,
            Expires = expires,
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
        };

    private static T GetValueOrException<T>(IConfiguration configuration, string key) =>
        configuration.GetValue<T?>(key) ?? throw new KeyNotFoundInConfigurationException(key);

    private static SigningCredentials CreateSigningCredentials(string key) =>
        new(CreateSymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

    private static SymmetricSecurityKey CreateSymmetricSecurityKey(string key) =>
        new(Encoding.UTF8.GetBytes(key));

    [LoggerMessage(Level = LogLevel.Error, Message = "{error}")]
    static partial void LogError(ILogger logger, string error);
}
