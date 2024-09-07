using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
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
    private readonly int _expirationInMinutes;

    private readonly SigningCredentials _signingCredentials;

    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        try
        {
            _issuer = GetValueOrException<string>(configuration, "Jwt:Issuer");
            _audience = GetValueOrException<string>(configuration, "Jwt:Audience");
            _expirationInMinutes = GetValueOrException<int>(configuration, "Jwt:ExpirationInMinutes");
            string secret = GetValueOrException<string>(configuration, "Jwt:Secret");
            _signingCredentials = CreateSigningCredentials(secret);
        }
        catch (Exception exception)
        {
            LogError(logger, exception.Message);
            throw;
        }
    }

    public string CreateToken(User user) =>
        _tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
            SigningCredentials = _signingCredentials,
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
        });

    private static T GetValueOrException<T>(IConfiguration configuration, string key) =>
        configuration.GetValue<T?>(key) ?? throw new KeyNotFoundInConfigurationException(key);

    private static SigningCredentials CreateSigningCredentials(string key) =>
        new(CreateSymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

    private static SymmetricSecurityKey CreateSymmetricSecurityKey(string key) =>
        new(Encoding.UTF8.GetBytes(key));

    [LoggerMessage(Level = LogLevel.Error, Message = "{error}")]
    static partial void LogError(ILogger logger, string error);
}