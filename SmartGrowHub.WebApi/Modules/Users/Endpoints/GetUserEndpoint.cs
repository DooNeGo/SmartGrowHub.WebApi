using Microsoft.AspNetCore.Authentication;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.WebApi.Modules.Users.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    private static readonly UnexpectedError NoTokenError =
        new("There is no access token in the headers");

    public static Task<IResult> GetUser(
        IUserRepository userRepository, IAccessTokenReader tokenReader,
        HttpContext context, ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) =>
        (from accessToken in GetAccessToken(context)
         from userId in tokenReader.GetUserId(accessToken).ToEff()
         from user in userRepository.GetById(userId, cancellationToken)
         select user)
            .RunAsync()
            .Map(fin => fin.Match(
                Succ: user => Ok(user.ToDto()),
                Fail: error => HandleError(logger, error)));

    private static Eff<AccessToken> GetAccessToken(HttpContext context) =>
        from rawToken in liftEff(() => context
            .GetTokenAsync("access_token")
            .Map(Optional))
            .Bind(option => option.ToEff(NoTokenError))
        from accessToken in AccessToken.From(rawToken).ToEff()
        select accessToken;
}