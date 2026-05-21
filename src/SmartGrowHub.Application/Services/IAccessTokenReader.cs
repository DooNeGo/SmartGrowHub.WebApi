using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Services;

public interface IAccessTokenReader
{
    Fin<Id<User>> GetUserId(AccessToken accessToken);
}
