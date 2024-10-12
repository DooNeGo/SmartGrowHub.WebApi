using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SmartGrowHub.WebApi.Infrastructure.Repositories.Extensions;

internal static class LocalViewExtensions
{
    public static Option<T> FindById<T>(this LocalView<T> localView, Ulid ulid)
        where T : class =>
        localView.FindEntry(ulid)?.Entity;
}