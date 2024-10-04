using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

internal static class UserSessionExtensions
{
    public static UserSessionDb ToDb(this UserSession session) =>
        new(session.Id, session.UserId,
            session.AuthTokens.AccessToken,
            session.AuthTokens.RefreshToken.Ulid,
            session.AuthTokens.RefreshToken.Expires);

    public static Fin<UserSession> TryToDomain(this UserSessionDb session) =>
        from accessToken in AccessToken.From(session.AccessToken)
        let refreshToken = new RefreshToken(session.RefreshToken, session.Expires)
        select new UserSession(
            new Id<UserSession>(session.Id),
            new Id<User>(session.UserId),
            new AuthTokens(accessToken, refreshToken));
}