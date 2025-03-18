using Microsoft.AspNetCore.Authentication;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.WebApi.Modules.Extensions;

public static class HttpContextExtensions
{
    private static readonly UnexpectedError NoTokenError =
        new("There is no access token in the headers");
    
    public static OptionT<IO, AccessToken> GetAccessToken(this HttpContext context) =>
        from rawToken in OptionT<IO, string>.LiftIO(
            IO.liftAsync(() => context.GetTokenAsync("access_token").Map(Prelude.Optional)))
        from accessToken in AccessToken.From(rawToken).ToIO()
        select accessToken;
}