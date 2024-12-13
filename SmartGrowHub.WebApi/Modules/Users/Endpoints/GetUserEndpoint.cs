using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.WebApi.Modules.Extensions;
using SmartGrowHub.WebApi.Modules.Users.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    public static Task<IResult> GetUser(
        IUserRepository userRepository, IAccessTokenReader tokenReader,
        HttpContext context, ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) => (
            from accessToken in context.GetAccessToken()
            from userId in tokenReader.GetUserId(accessToken).ToEff()
            from user in userRepository.GetById(userId, cancellationToken)
            select user)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: user => Ok(user.ToDto()),
            Fail: error => HandleError(logger, error)));
}