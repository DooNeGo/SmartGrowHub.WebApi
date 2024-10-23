using Microsoft.IdentityModel.JsonWebTokens;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class AccessTokenReader : IAccessTokenReader
{
    private static readonly JsonWebTokenHandler TokenHandler = new();

    public Fin<Id<User>> GetUserId(AccessToken accessToken) =>
        Domain.Common.Id<User>.From(TokenHandler.ReadJsonWebToken(accessToken).Subject);
}
