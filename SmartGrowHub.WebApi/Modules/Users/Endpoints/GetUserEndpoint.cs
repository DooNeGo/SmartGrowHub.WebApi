using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.Users.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    public static Task<IResult> GetUser(
        string id,
        IUserRepository userService,
        ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) =>
        (from userId in Domain.Common.Id<User>.From(id).ToEff()
         from user in userService.GetById(userId, cancellationToken)
         select user)
            .RunAsync()
            .Map(fin => fin.Match(
                Succ: user => Ok(user.ToDto()),
                Fail: error => HandleError(logger, error.ToException())));
}