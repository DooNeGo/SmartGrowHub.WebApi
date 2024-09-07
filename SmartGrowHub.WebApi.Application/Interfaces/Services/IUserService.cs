using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IUserService
{
    EitherAsync<Exception, Unit> AddAsync(User user, CancellationToken cancellationToken);
    EitherAsync<Exception, User> GetAsync(UserName userName, CancellationToken cancellationToken);
    EitherAsync<Exception, User> GetAsync(Id<User> id, CancellationToken cancellationToken);
}