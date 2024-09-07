using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Repositories;
using SmartGrowHub.WebApi.Infrastructure.Data;
using SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationContext context) : IUserRepository
{
    public Try<Unit> Add(User user) => () =>
    {
        context.Users.Add(user.ToDb());
        return unit;
    };

    public TryOptionAsync<Fin<User>> GetAsync(UserName userName, CancellationToken cancellationToken) =>
        TryOptionAsync(() => context.Users
            .Where(user => user.UserName == userName.Value)
            .FirstOrDefaultAsync(cancellationToken)
            .Map(Optional)
            .Map(option => option
                .Map(user => user.TryToDomain())));

    public TryOptionAsync<Fin<User>> GetAsync(Id<User> id, CancellationToken cancellationToken) =>
        TryOptionAsync(() => context.Users
            .FindAsync([id.Value], cancellationToken)
            .AsTask()
            .Map(Optional)
            .Map(option => option
                .Map(user => user.TryToDomain())));

    public TryAsync<Unit> SaveChangesAsync(CancellationToken cancellationToken) =>
        TryAsync(() => context
            .SaveChangesAsync(cancellationToken)
            .ToUnit());
}