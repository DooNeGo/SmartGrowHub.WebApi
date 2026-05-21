namespace SmartGrowHub.Domain.Common;

public sealed record AuthTokens(
    AccessToken AccessToken,
    RefreshToken RefreshToken);