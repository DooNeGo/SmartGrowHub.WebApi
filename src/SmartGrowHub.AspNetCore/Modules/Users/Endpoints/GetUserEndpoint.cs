using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.AspNetCore.Modules.Users.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    public static ValueTask<IResult> GetUser(
        IUserRepository userRepository, IAccessTokenReader tokenReader,
        HttpContext context, ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) => (
            from userId in tokenReader.GetUserId(context)
            from user in userRepository
                .GetById(userId, cancellationToken)
                .ToIOOrFail(DomainErrors.UserNotFoundError)
            select user)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            Succ: user => Ok(user.ToDto()),
            Fail: error => HandleError(logger, error)));
}