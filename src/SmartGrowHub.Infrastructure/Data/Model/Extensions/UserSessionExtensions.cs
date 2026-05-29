using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class UserSessionExtensions
{
    public static UserSessionDb ToDb(this UserSession session) => new()
    {
        Id = session.Id,
        UserId = session.UserId,
        AccessToken = session.AuthTokens.AccessToken,
        RefreshToken = session.AuthTokens.RefreshToken.Value,
        Expires = session.AuthTokens.RefreshToken.Expires
    };

    public static List<UserSessionDb> ToDb(this ImmutableArray<UserSession> array) =>
        array.Select(session => session.ToDb()).ToList();

    public static Fin<UserSession> TryToDomain(this UserSessionDb session) =>
        from accessToken in AccessToken.From(session.AccessToken)
        from refreshToken in RefreshToken.From((session.RefreshToken, session.Expires))
        from sessionId in Id<UserSession>.From(session.Id)
        from userId in Id<User>.From(session.UserId)
        select new UserSession(sessionId, userId, new AuthTokens(accessToken, refreshToken));
}