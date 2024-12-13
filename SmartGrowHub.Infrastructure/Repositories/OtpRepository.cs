using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class OtpRepository(ApplicationContext context) : IOtpRepository
{
    public Eff<Unit> Add(OneTimePassword oneTimePassword, CancellationToken cancellationToken) =>
        Add(oneTimePassword) >> SaveChanges(cancellationToken);

    public Eff<ImmutableArray<OneTimePassword>> GetAllByUserId(Id<User> id, CancellationToken cancellationToken) =>
        liftEff(() => context.OneTimePasswords
                .Where(otp => otp.UserId == id.Value)
                .ToListAsync(cancellationToken))
            .Bind(list => list
                .AsIterable()
                .Traverse(otp => otp.ToDomain())
                .Map(iterable => iterable.ToImmutableArray())
                .As().ToEff());
    
    public Eff<Unit> Remove(Id<OneTimePassword> id, CancellationToken cancellationToken) =>
        liftEff(() => context.OneTimePasswords
            .Where(otp => otp.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ToUnit());

    public Eff<Unit> Remove(OneTimePassword oneTimePassword, CancellationToken cancellationToken) =>
        Remove(oneTimePassword) >> SaveChanges(cancellationToken);

    public Eff<Unit> Update(OneTimePassword oneTimePassword, CancellationToken cancellationToken) =>
        Update(oneTimePassword) >> SaveChanges(cancellationToken);

    private Eff<Unit> Add(OneTimePassword oneTimePassword) =>
        liftEff(() => context.OneTimePasswords.Add(oneTimePassword.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> Remove(OneTimePassword oneTimePassword) =>
        liftEff(() => context.OneTimePasswords.Remove(oneTimePassword.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> Update(OneTimePassword oneTimePassword) =>
        liftEff(() => context.OneTimePasswords.Update(oneTimePassword.ToDb()))
            .Map(_ => unit);

    private Eff<Unit> SaveChanges(CancellationToken cancellationToken) =>
        liftEff(() => context.SaveChangesAsync(cancellationToken))
            .Map(_ => unit);
}