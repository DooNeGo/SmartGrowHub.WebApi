using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.Users.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ExceptionHandler;

namespace SmartGrowHub.WebApi.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    public static Task<IResult> GetUser(
        Ulid id,
        IUserService userService,
        ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) =>
        Id(new Id<User>(in id))
            .Map(id => userService.GetAsync(id, cancellationToken))
            .Map(either => either.Match(
                Right: user => Ok(user.ToDto()),
                Left: exception => HandleException(logger, exception)))
            .Value;
}