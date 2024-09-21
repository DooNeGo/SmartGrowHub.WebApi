using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IPasswordHasher
{
    Password Hash(Password password);
    bool Verify(Password password, string passwordHash);
}