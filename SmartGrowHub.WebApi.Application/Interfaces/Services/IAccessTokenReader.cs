using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IAccessTokenReader
{
    public Fin<Id<User>> GetUserId(AccessToken accessToken);
}
