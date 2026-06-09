using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class OtpRepository : Repository<OneTimePassword, OneTimePasswordDb>, IOtpRepository
{
    public OtpRepository(ApplicationContext context) : base(context) { }

    public OptionT<IO, OneTimePassword> GetByValue(NonEmptyString value) =>
        GetByPredicate(otp => otp.Value == value);

    protected override OneTimePasswordDb ToDb(OneTimePassword domain) => domain.ToDb();

    protected override Fin<OneTimePassword> ToDomain(OneTimePasswordDb db) => db.ToDomain();

    protected override IQueryable<OneTimePasswordDb> AddIncludes(IQueryable<OneTimePasswordDb> query) => query;
}