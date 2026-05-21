using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.OneTimePasswords;

public sealed record OtpConfiguration(
    NonNegativeInteger Length,
    TimeSpan Expiration);