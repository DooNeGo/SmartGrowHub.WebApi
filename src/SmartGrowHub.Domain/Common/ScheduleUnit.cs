using System.Numerics;
using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Domain.Common;

public enum ScheduleUnitKind
{
    Prefer,
    Power
}

public sealed class ScheduleUnit<T> : Entity<ScheduleUnit<T>>
    where T : IComparisonOperators<T, T, bool>, ISubtractionOperators<T, T, TimeSpan>
{
    private ScheduleUnit(
        Id<ScheduleUnit<T>> id,
        Id<ModuleSchedule> scheduleId,
        ScheduleUnitKind kind,
        Quantity quantity,
        TimeInterval<T> timeInterval)
        : base(id)
    {
        ScheduleId = scheduleId;
        Kind = kind;
        Quantity = quantity;
        TimeInterval = timeInterval;
    }

    public Id<ModuleSchedule> ScheduleId { get; }
    
    public ScheduleUnitKind Kind { get; }

    public Quantity Quantity { get; }

    public TimeInterval<T> TimeInterval { get; }

    public static Fin<ScheduleUnit<T>> New(Id<ModuleSchedule> scheduleId, ScheduleUnitKind kind, Quantity quantity,
        TimeInterval<T> timeInterval, Id<ScheduleUnit<T>>? id = null) =>
        from _ in kind is ScheduleUnitKind.Power ? ValidatePowerQuantity(quantity) : Fin.Succ(Unit.Default)
        select new ScheduleUnit<T>(id ?? new Id<ScheduleUnit<T>>(), scheduleId, kind, quantity, timeInterval);

    private static Fin<Unit> ValidatePowerQuantity(Quantity quantity) =>
        quantity is { Magnitude: < 0 or > 100, Unit: MeasurementUnit.Percent }
            ? DomainErrors.CreateQuantityOutOfRangeError(quantity, 0, 100)
            : Fin.Succ(Unit.Default);
}