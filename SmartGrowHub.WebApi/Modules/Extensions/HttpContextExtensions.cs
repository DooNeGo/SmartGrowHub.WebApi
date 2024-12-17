using Microsoft.AspNetCore.Authentication;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;

namespace SmartGrowHub.WebApi.Modules.Extensions;

public static class HttpContextExtensions
{
    private static readonly UnexpectedError NoTokenError =
        new("There is no access token in the headers");
    
    public static Eff<AccessToken> GetAccessToken(this HttpContext context) =>
        from rawToken in liftEff(() => context
                .GetTokenAsync("access_token")
                .Map(Optional))
            .Bind(option => option.ToEff(NoTokenError))
        from accessToken in AccessToken.From(rawToken).ToEff()
        select accessToken;
}