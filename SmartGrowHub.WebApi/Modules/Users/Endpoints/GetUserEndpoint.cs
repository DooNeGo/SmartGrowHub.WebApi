using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared;
using SmartGrowHub.Shared.Users.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    public static Task<IResult> GetUser(
        string id,
        IUserService userService,
        ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) =>
        (from userId in UlidFP.TryCreate(id).ToEff()
         from user in userService.GetAsync(new Id<User>(userId), cancellationToken)
         select user)
            .RunAsync()
            .Map(fin => fin.Match(
                Succ: user => Ok(user.ToDto()),
                Fail: error => HandleException(logger, error.ToException())));
}