using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IPasswordHasher
{
    Fin<Password> Hash(Password password);
    Fin<bool> Verify(Password password, Password hasedPassword);
}