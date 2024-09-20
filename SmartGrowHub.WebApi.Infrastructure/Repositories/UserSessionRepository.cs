using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories;

internal sealed class UserSessionRepository(ApplicationContext context) : IUserSessionRepository
{
    public Either<InternalException, Unit> Add(UserSession userSession) =>
        Try(() => context.Add(userSession.ToDb()))
            .ToEither(exception => new InternalException(exception))
            .Map(_ => unit);

    public EitherAsync<Exception, UserSession> GetAsync(RefreshToken token,
        CancellationToken cancellationToken) =>
        TryOptionAsync(() => context.UserSessions
            .Where(session => session.RefreshToken == token.Value)
            .FirstOrDefaultAsync(cancellationToken)
            .Map(Optional))
        .ToEither(exception => (Exception)new InternalException(exception))
        .ToAsync()
        .Bind(opt => opt.Match(
            Some: session => session
                .TryToDomain()
                .ToEitherAsync()
                .MapLeft(error => error.ToException()),
            None: () => new ItemNotFoundException(nameof(UserSession), None)));

    public EitherAsync<InternalException, Unit> RemoveAllAsync(Id<User> id,
        CancellationToken cancellationToken) =>
        TryAsync(() => context.UserSessions
            .Where(session => session.UserId == id.Value)
            .ExecuteDeleteAsync(cancellationToken))
            .ToEither(error => new InternalException(error.ToException()))
            .Map(_ => unit);

    public EitherAsync<Exception, Unit> SaveChangesAsync(CancellationToken cancellationToken) =>
        TryAsync(() => context
            .SaveChangesAsync(cancellationToken)
            .Map(_ => unit))
            .ToEither(error => error.ToException());

    public EitherAsync<InternalException, Unit> UpdateAsync(UserSession userSession,
        CancellationToken cancellationToken) =>
        TryAsync(() => context.UserSessions
            .Where(session => session.Id == userSession.Id)
            .Select(oldSession => new
            {
                oldSession,
                RefreshToken = userSession.AuthTokens.RefreshToken.Value.Value,
                AccessToken = userSession.AuthTokens.AccessToken.Value.Value
            })
            .ExecuteUpdateAsync(properties => properties
                .SetProperty(
                    tuple => tuple.oldSession.AccessToken,
                    tuple => tuple.AccessToken)
                .SetProperty(
                    tuple => tuple.oldSession.RefreshToken,
                    tuple => tuple.RefreshToken),
                cancellationToken)
            .Map(_ => unit))
            .ToEither(error => new InternalException(error.ToException()));
}
