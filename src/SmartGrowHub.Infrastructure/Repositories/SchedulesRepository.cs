using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Model.Programs;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class SchedulesRepository : Repository<ModuleSchedule, ScheduleDb>, ISchedulesRepository
{
    public SchedulesRepository(ApplicationContext context) : base(context) { }

    protected override ScheduleDb ToDb(ModuleSchedule domain) => domain.ToDb();

    protected override Fin<ModuleSchedule> ToDomain(ScheduleDb db) => db.ToDomain();

    protected override IQueryable<ScheduleDb> AddIncludes(IQueryable<ScheduleDb> query) => query.Include(x => x.Units);
}