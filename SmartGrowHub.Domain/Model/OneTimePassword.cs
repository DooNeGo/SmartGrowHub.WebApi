using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model;

public sealed class OneTimePassword(
    Id<OneTimePassword> id,
    Id<User> userId,
    NonEmptyString value,
    DateTime expires)
    : Entity<OneTimePassword>(id)
{
    public Id<User> UserId { get; init; } = userId;
    
    public NonEmptyString Value { get; init; } = value;
    
    public DateTime Expires { get; init; } = expires;
    
    public static OneTimePassword New(Id<User> userId, NonEmptyString value, DateTime expires) =>
        new(new Id<OneTimePassword>(), userId, value, expires);

    public bool IsExpired(DateTime now) => now > Expires;
}