using SmartGrowHub.Domain.Abstractions;

namespace SmartGrowHub.Domain.Common;

public readonly record struct TimeOnlyWrapper(TimeOnly Inner)
    : IComparable<TimeOnlyWrapper>, IArithmetic<TimeOnlyWrapper, TimeSpan>
{
    public int CompareTo(TimeOnlyWrapper other) => Inner.CompareTo(other.Inner);

    public TimeSpan Subtract(TimeOnlyWrapper other) => Inner - other.Inner;
}