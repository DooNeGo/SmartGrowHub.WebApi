namespace SmartGrowHub.Infrastructure.Tokens;

public sealed record TokensConfiguration(
    AccessTokenConfiguration AccessTokenConfiguration,
    RefreshTokenConfiguration RefreshTokenConfiguration);
