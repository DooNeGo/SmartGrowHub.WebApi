using Microsoft.IdentityModel.Tokens;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Tokens;

public sealed record AccessTokenConfiguration(
    NonEmptyString Issuer,
    NonEmptyString Audience,
    SigningCredentials SigningCredentials,
    TimeSpan AccessTokenExpiration);
