using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Repositories;

public interface IUserSessionRepository
{
    Either<InternalException, Unit> Add(UserSession userSession);
    EitherAsync<Exception, UserSession> GetAsync(RefreshToken token, CancellationToken cancellationToken);
    EitherAsync<InternalException, Unit> RemoveAllAsync(Id<User> id, CancellationToken cancellationToken);
    EitherAsync<Exception, Unit> SaveChangesAsync(CancellationToken cancellationToken);
    EitherAsync<InternalException, Unit> UpdateAsync(UserSession userSession, CancellationToken cancellationToken);
}