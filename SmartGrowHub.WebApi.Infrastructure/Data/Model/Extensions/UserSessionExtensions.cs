using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

internal static class UserSessionExtensions
{
    public static UserSessionDb ToDb(this UserSession session) =>
        new(session.Id, session.UserId,
            session.AuthTokens.AccessToken,
            session.AuthTokens.RefreshToken);

    public static Fin<UserSession> TryToDomain(this UserSessionDb session) =>
        UserSession.Create(session.Id, session.UserId, session.AccessToken, session.RefreshToken);
}