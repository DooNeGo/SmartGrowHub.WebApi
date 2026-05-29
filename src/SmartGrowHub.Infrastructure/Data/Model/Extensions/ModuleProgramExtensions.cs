using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model.GrowHub.Programs;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class ModuleProgramExtensions
{
    public static ModuleProgramDb ToDb(this ModuleProgram program)
    {
        var programDb = new ModuleProgramDb
        {
            Id = program.Id,
            Type = ProgramTypeDb.Disabled
        };

        program.Match(
            _ => programDb.Type = ProgramTypeDb.Disabled,
            manual =>
            {
                programDb.ManualQuantity = manual.Quantity.ToDb();
                return programDb.Type = ProgramTypeDb.Manual;
            },
            daily =>
            {
                programDb.TimeOnlyEntries = daily.Entries.Select(x => x.ToDb()).ToList();
                return programDb.Type = ProgramTypeDb.Daily;
            },
            weekly =>
            {
                programDb.WeekTimeOnlyEntries = weekly.Entries.Select(x => x.ToDb()).ToList();
                return programDb.Type = ProgramTypeDb.Weekly;
            });
        
        return programDb;
    }

    public static Fin<ModuleProgram> ToDomain(this ModuleProgramDb programDb) =>
        from id in Id<ModuleProgram>.From(programDb.Id)
        from program in programDb.Type switch
        {
            ProgramTypeDb.Disabled => Fin.Succ<ModuleProgram>(new DisabledProgram(id)),
            ProgramTypeDb.Manual => programDb.ManualQuantity is not null
                ? ManualProgram.New(programDb.ManualQuantity.ToDomain(), id).Map(ModuleProgram (x) => x)
                : Fin.Fail<ModuleProgram>("Manual quantity was null for manual program"),
            ProgramTypeDb.Daily => DailyProgram
                .New(programDb.TimeOnlyEntries.Select(x => x.ToDomain()).ToImmutableList(), id)
                .Map(ModuleProgram (x) => x),
            ProgramTypeDb.Weekly => WeeklyProgram
                .New(programDb.WeekTimeOnlyEntries.Select(x => x.ToDomain()).ToImmutableList(), id)
                .Map(ModuleProgram (x) => x),
            _ => throw new ArgumentOutOfRangeException(nameof(programDb), programDb.Type, null)
        }
        select program;
}