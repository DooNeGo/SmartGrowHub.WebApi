using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IPasswordHasher
{
    Fin<Password> TryHash(Password password);
    Fin<bool> TryVerify(Password password, Password hasedPassword);
}