using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.UserSessions.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.UserSessions.Endpoints;

public sealed class GetUserSessionsEndpoint
{
    public static Task<IResult> GetUserSessions(
        string? userId, IUserSessionRepository sessionRepository,
        ILogger<GetUserSessionsEndpoint> logger, CancellationToken cancellationToken) =>
        (from id in Domain.Common.Id<User>.From(userId ?? string.Empty).ToEff()
         from sessions in sessionRepository.GetAllByUserId(id, cancellationToken)
         select sessions)
            .RunAsync()
            .Map(fin => fin.Match(
                Succ: sessions => Ok(sessions.ToDto()),
                Fail: error => HandleError(logger, error)));
}
