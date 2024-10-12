namespace SmartGrowHub.WebApi.Infrastructure.Tokens;

public sealed record TokensConfiguration(
    AccessTokenConfiguration AccessTokenConfiguration,
    RefreshTokenConfiguration RefreshTokenConfiguration);
