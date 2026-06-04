using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Model.Programs;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Extensions;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class SchedulesRepository : Repository<ModuleSchedule, ScheduleDb>, ISchedulesRepository
{
    private readonly ApplicationContext _context;
    
    public SchedulesRepository(ApplicationContext context) : base(context) => _context = context;

    public override IO<Unit> Update(ModuleSchedule domain) =>
        from option in IO.lift(() => Prelude.Optional(_context.Schedules.Local.FindEntry(domain.Id.Value)))
        let newDb = ToDb(domain)
        from _1 in option.Match(
            Some: entry => IO.lift(() =>
            {
                entry.CurrentValues.SetValues(newDb);
                entry.Entity.Units.Clear();
                
                foreach (ScheduleUnitDb unit in newDb.Units)
                {
                    entry.Entity.Units.Add(unit);
                }
            }),
            None: () => _context.UpdateIO(newDb))
        select _1;

    protected override ScheduleDb ToDb(ModuleSchedule domain) => domain.ToDb();

    protected override Fin<ModuleSchedule> ToDomain(ScheduleDb db) => db.ToDomain();

    protected override IQueryable<ScheduleDb> AddIncludes(IQueryable<ScheduleDb> query) => query.Include(x => x.Units);
}