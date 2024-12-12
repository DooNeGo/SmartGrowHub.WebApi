using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class UserSessionExtensions
{
    public static UserSessionDb ToDb(this UserSession session) => new()
    {
        Id = session.Id,
        UserDbId = session.UserId,
        AccessToken = session.AuthTokens.AccessToken,
        RefreshToken = session.AuthTokens.RefreshToken.Ulid,
        Expires = session.AuthTokens.RefreshToken.Expires
    };

    public static List<UserSessionDb> ToDb(this ImmutableArray<UserSession> array) =>
        array.Select(session => session.ToDb()).ToList();

    public static Fin<UserSession> TryToDomain(this UserSessionDb session) =>
        from accessToken in AccessToken.From(session.AccessToken)
        let refreshToken = new RefreshToken(session.RefreshToken, session.Expires)
        select new UserSession(
            new Id<UserSession>(session.Id),
            new Id<User>(session.UserDbId),
            new AuthTokens(accessToken, refreshToken));
}