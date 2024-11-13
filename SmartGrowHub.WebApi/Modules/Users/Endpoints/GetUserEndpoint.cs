using Microsoft.AspNetCore.Authentication;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.Users.Extensions;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
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
        from rawToken in context
            .GetTokenAsync("access_token")
            .Map(Optional).ToEff()
            .Bind(option => option.ToEff(NoTokenError))
        from accessToken in AccessToken.From(rawToken).ToEff()
        select accessToken;
}