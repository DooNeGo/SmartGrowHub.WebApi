using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Domain.Model.Programs;

public sealed class DailyProgram : ModuleProgram
{
    private DailyProgram(Id<ModuleProgram> id, ImmutableList<TimedQuantity<TimeOnlyWrapper>> entries) : base(id) =>
        Entries = entries;

    public ImmutableList<TimedQuantity<TimeOnlyWrapper>> Entries { get; }

    public static Fin<DailyProgram> New(ImmutableList<TimedQuantity<TimeOnlyWrapper>> entries,
        Id<ModuleProgram>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromHours(TimeSpan.HoursPerDay))
            return Error.New($"Duration must be less than or equal {TimeSpan.HoursPerDay} hours");

        return entries
            .AsIterable()
            .Traverse(entry => QuantityDefaults.ValidatePowerPercentRange(entry.Quantity))
            .Map(_ => new DailyProgram(id ?? new Id<ModuleProgram>(), entries))
            .As();
    }
}