using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface ITokenService
{
    string CreateToken(User user);
}