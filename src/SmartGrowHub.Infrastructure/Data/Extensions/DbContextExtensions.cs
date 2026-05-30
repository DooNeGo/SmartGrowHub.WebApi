using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Infrastructure.Data.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class DbContextExtensions
{
    extension(DbContext context)
    {
        public IO<Unit> AddIO<T>(T entity) where T : class =>
            IO.lift(() => context.Add(entity)).ToUnit();

        public IO<Unit> RemoveIO<T>(T entity) where T : class =>
            IO.lift(() => context.Remove(entity)).ToUnit();

        public IO<Unit> UpdateIO<T>(T entity) where T : class =>
            IO.lift(() => context.Update(entity)).ToUnit();

        public IO<Unit> SaveChangesIO(CancellationToken cancellationToken) =>
            IO.liftAsync(() => context.SaveChangesAsync(cancellationToken)).ToUnit();
    }
}