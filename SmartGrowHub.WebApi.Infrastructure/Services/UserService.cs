using EntityFramework.Exceptions.Common;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class UserService(IUserRepository userRepository) : IUserService
{
    public Eff<Unit> AddAsync(User user, CancellationToken cancellationToken) =>
        userRepository.Add(user)
            .Bind(_ => userRepository.SaveChangesAsync(cancellationToken))
            .MapFail(error => Id(error.ToException())
                .Map<Exception>(exception => exception is UniqueConstraintException
                    ? new UserAlreadyExistException()
                    : new InternalException(exception))
                .Value);

    public Eff<User> GetAsync(UserName userName, CancellationToken cancellationToken) =>
        userRepository.GetAsync(userName, cancellationToken);

    public Eff<User> GetAsync(Id<User> id, CancellationToken cancellationToken) =>
        userRepository.GetAsync(id, cancellationToken);
}
