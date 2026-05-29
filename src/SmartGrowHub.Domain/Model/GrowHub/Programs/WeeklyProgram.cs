using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Domain.Model.GrowHub.Programs;

public sealed class WeeklyProgram : ModuleProgram
{
    private const int DaysInWeek = 7;
    
    private WeeklyProgram(Id<ModuleProgram> id, ImmutableList<TimedQuantity<WeekTimeOnly>> entries) : base(id) =>
        Entries = entries;
    
    public ImmutableList<TimedQuantity<WeekTimeOnly>> Entries { get; }

    public static Fin<WeeklyProgram> New(ImmutableList<TimedQuantity<WeekTimeOnly>> entries,
        Id<ModuleProgram>? id = null)
    {
        if (entries.HasOverlappingIntervals()) return Error.New("Intervals must not overlap");
        if (entries.CalculateTimeInterval().Duration > TimeSpan.FromDays(DaysInWeek))
            return Error.New($"Duration must be less than or equal {DaysInWeek} days");

        return entries
            .AsIterable()
            .Traverse(entry => QuantityDefaults.ValidatePowerPercentRange(entry.Quantity))
            .Map(_ => new WeeklyProgram(id ?? new Id<ModuleProgram>(), entries))
            .As();
    }
}