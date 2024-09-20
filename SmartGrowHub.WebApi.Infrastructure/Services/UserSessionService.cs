﻿using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class UserSessionService(
    IUserSessionRepository sessionRepository,
    IUserService userService,
    ITokenService tokenService)
    : IUserSessionService
{
    public EitherAsync<Exception, UserSession> CreateAsync(User user, CancellationToken cancellationToken)
        => sessionRepository.RemoveAllAsync(user.Id, cancellationToken)
            .MapLeft(exception => (Exception)exception)
            .Map(_ => tokenService.CreateTokens(user))
            .Map(tokens => UserSession.Create(user.Id, tokens))
            .Bind(session => sessionRepository
                .Add(session).ToAsync().MapLeft(exception => (Exception)exception)
                .Bind(_ => sessionRepository.SaveChangesAsync(cancellationToken))
                .Map(_ => session));

    public EitherAsync<Exception, RefreshTokensResponse> RefreshTokensAsync(
        RefreshToken refreshToken, CancellationToken cancellationToken) =>
        sessionRepository.GetAsync(refreshToken, cancellationToken)
            .Bind(session => userService.GetAsync(session.UserId, cancellationToken)
                .Map(user => tokenService.CreateTokens(user))
                .Bind(tokens => sessionRepository
                    .UpdateAsync(session with { AuthTokens = tokens }, cancellationToken)
                    .MapLeft(exception => (Exception)exception)
                    .Map(_ => new RefreshTokensResponse(tokens))));
}