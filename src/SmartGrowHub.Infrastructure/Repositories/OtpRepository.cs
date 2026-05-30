using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class OtpRepository : Repository<OneTimePassword, OneTimePasswordDb>, IOtpRepository
{
    private readonly ApplicationContext _context;

    public OtpRepository(ApplicationContext context) : base(context) => _context = context;

    public OptionT<IO, OneTimePassword> GetByValue(NonEmptyString value, CancellationToken cancellationToken) =>
        from otp in OptionT.liftIO<IO, OneTimePasswordDb>(
            IO.liftAsync(() =>
                _context.OneTimePasswords
                    .Where(otp => otp.Value == value)
                    .FirstOrDefaultAsync(cancellationToken)
                    .Map(Prelude.Optional)))
        from domainOtp in otp.ToDomain().ToIO()
        select domainOtp;

    protected override OneTimePasswordDb ToDb(OneTimePassword domain) => domain.ToDb();

    protected override Fin<OneTimePassword> ToDomain(OneTimePasswordDb db) => db.ToDomain();

    protected override IQueryable<OneTimePasswordDb> AddIncludes(IQueryable<OneTimePasswordDb> query) => query;
}