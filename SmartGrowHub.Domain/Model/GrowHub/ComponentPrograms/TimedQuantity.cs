using SmartGrowHub.Domain.Abstractions;
using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.ComponentPrograms;

public sealed class TimedQuantity<TTime> : Entity<TimedQuantity<TTime>>
    where TTime : IComparable<TTime>, IArithmetic<TTime, TimeSpan>
{
    private TimedQuantity(Id<TimedQuantity<TTime>> id, TimeInterval<TTime> interval, Quantity quantity)
        : base(id) => (TimeInterval, Quantity) = (interval, quantity);

    public Quantity Quantity { get; }
    
    public TimeInterval<TTime> TimeInterval { get; }

    public static Fin<TimedQuantity<TTime>> New(Quantity quantity, TimeInterval<TTime> interval) =>
        interval.Duration < TimeSpan.FromMinutes(1)
            ? Error.New("Duration must be at least 1 minute")
            : new TimedQuantity<TTime>(new Id<TimedQuantity<TTime>>(), interval, quantity);
}