using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class OtpRepository(ApplicationContext context) : IOtpRepository
{
    public IO<Unit> Add(OneTimePassword oneTimePassword, CancellationToken cancellationToken) =>
        Add(oneTimePassword) >> SaveChanges(cancellationToken);

    public IO<ImmutableArray<OneTimePassword>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.OneTimePasswords
                .Where(otp => otp.UserId == id.Value)
                .ToListAsync(cancellationToken))
            .Bind(list => list
                .AsIterable()
                .Traverse(otp => otp.ToDomain())
                .Map(iterable => iterable.ToImmutableArray())
                .As().ToIO());

    public OptionT<IO, OneTimePassword> GetByValue(NonEmptyString value, CancellationToken cancellationToken) =>
        from otp in OptionT<IO, OneTimePasswordDb>.LiftIO(
            IO.liftAsync(() =>
                context.OneTimePasswords
                    .Where(otp => otp.Value == value)
                    .FirstOrDefaultAsync(cancellationToken)
                    .Map(Prelude.Optional)))
        from domainOtp in otp.ToDomain().ToIO()
        select domainOtp;

    public IO<Unit> Remove(Id<OneTimePassword> id, CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.OneTimePasswords
            .Where(otp => otp.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public IO<Unit> Remove(OneTimePassword oneTimePassword, CancellationToken cancellationToken) =>
        Remove(oneTimePassword) >> SaveChanges(cancellationToken);

    public IO<Unit> Update(OneTimePassword oneTimePassword, CancellationToken cancellationToken) =>
        Update(oneTimePassword) >> SaveChanges(cancellationToken);

    private IO<Unit> Add(OneTimePassword oneTimePassword) =>
        IO.lift(() => context.OneTimePasswords.Add(oneTimePassword.ToDb()))
            .ToUnit();

    private IO<Unit> Remove(OneTimePassword oneTimePassword) =>
        IO.lift(() => context.OneTimePasswords.Remove(oneTimePassword.ToDb()))
            .ToUnit();

    private IO<Unit> Update(OneTimePassword oneTimePassword) =>
        IO.lift(() => context.OneTimePasswords.Update(oneTimePassword.ToDb()))
            .ToUnit();

    private IO<Unit> SaveChanges(CancellationToken cancellationToken) =>
        IO.liftAsync(() => context.SaveChangesAsync(cancellationToken))
            .ToUnit();
}