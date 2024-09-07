namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IPasswordHasher
{
    string GetHash(string password);
    bool Verify(string password, string passwordHash);
}