using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class UserService(IUserRepository userRepository) : IUserService
{
    public EitherAsync<Exception, Unit> AddAsync(User user, CancellationToken cancellationToken) =>
        userRepository
            .Add(user).ToAsync()
            .Bind(_ => userRepository.SaveChangesAsync(cancellationToken))
            .Match(
                Succ: unit => (Either<Exception, Unit>)unit,
                Fail: exception => exception is DbUpdateException
                    ? new UserAlreadyExistException()
                    : new InternalException(exception))
            .ToAsync();

    public EitherAsync<Exception, User> GetAsync(UserName userName, CancellationToken cancellationToken) =>
        userRepository
            .GetAsync(userName, cancellationToken)
            .Match(
                Some: fin => fin.Match(
                    Succ: user => (Either<Exception, User>)user,
                    Fail: error => new InternalException(error.ToException())),
                None: () => new ItemNotFoundException(nameof(User), None),
                Fail: exception => new InternalException(exception))
            .ToAsync();

    public EitherAsync<Exception, User> GetAsync(Id<User> id, CancellationToken cancellationToken) =>
        userRepository
            .GetAsync(id, cancellationToken)
            .Match(
                Some: fin => fin.Match(
                    Succ: user => (Either<Exception, User>)user,
                    Fail: error => new InternalException(error.ToException())),
                None: () => new ItemNotFoundException(nameof(User), None),
                Fail: exception => exception)
            .ToAsync();
}
