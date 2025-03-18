using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Services;

public interface IUserService
{
    IO<UserSession> AddNewSessionToUser(User user, CancellationToken cancellationToken);
}