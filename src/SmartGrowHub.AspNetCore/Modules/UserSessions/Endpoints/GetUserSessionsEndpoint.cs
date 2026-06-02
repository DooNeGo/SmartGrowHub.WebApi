namespace SmartGrowHub.AspNetCore.Modules.UserSessions.Endpoints;

public sealed class GetUserSessionsEndpoint
{
    // public static Task<IResult> GetUserSessions(
    //     HttpContext context, IAccessTokenReader tokenReader, IUserSessionRepository sessionRepository,
    //     ILogger<GetUserSessionsEndpoint> logger, CancellationToken cancellationToken) => (
    //         from token in IO.token
    //         from userId in tokenReader.GetUserId(context)
    //         from id in Domain.Common.Id<User>.From(userId).ToIO()
    //      from sessions in sessionRepository.GetAllByUserId(id, token)
    //      select sessions)
    //         .RunSafeAsync(EnvIO.New(token: cancellationToken))
    //         .Map(fin => fin.Match(
    //             Succ: sessions => Ok(sessions.ToDto()),
    //             Fail: error => HandleError(logger, error)));
}
