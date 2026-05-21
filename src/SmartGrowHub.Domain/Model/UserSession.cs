using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;

namespace SmartGrowHub.Domain.Model;

public sealed class UserSession(
    Id<UserSession> id,
    Id<User> userId,
    AuthTokens authTokens)
    : Entity<UserSession>(id)
{
    private UserSession(UserSession original) : this(
        original.Id, original.UserId, original.AuthTokens)
    { }

    public Id<User> UserId { get; init; } = userId;

    public AuthTokens AuthTokens { get; init; } = authTokens;

    public static UserSession New(Id<User> userId, AuthTokens tokens) =>
        new(new Id<UserSession>(), userId, tokens);

    public Fin<UserSession> UpdateTokens(AuthTokens tokens, DateTime now) =>
        AuthTokens.RefreshToken.Expires <= now
            ? DomainErrors.SessionExpiredError
            : new UserSession(this) { AuthTokens = tokens };
}
