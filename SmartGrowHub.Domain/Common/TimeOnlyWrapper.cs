using System.Numerics;
using SmartGrowHub.Domain.Abstractions;

namespace SmartGrowHub.Domain.Common;

public readonly record struct TimeOnlyWrapper(TimeOnly Inner) :
    IComparable<TimeOnlyWrapper>,
    ISubtractionOperators<TimeOnlyWrapper, TimeOnlyWrapper, TimeSpan>,
    IComparisonOperators<TimeOnlyWrapper, TimeOnlyWrapper, bool>
{
    public int CompareTo(TimeOnlyWrapper other) => Inner.CompareTo(other.Inner);

    public static TimeSpan operator -(TimeOnlyWrapper left, TimeOnlyWrapper right) => left.Inner - right.Inner;
    
    public static bool operator >(TimeOnlyWrapper left, TimeOnlyWrapper right) => left.Inner > right.Inner;

    public static bool operator >=(TimeOnlyWrapper left, TimeOnlyWrapper right) => left.Inner >= right.Inner;

    public static bool operator <(TimeOnlyWrapper left, TimeOnlyWrapper right) => left.Inner < right.Inner;

    public static bool operator <=(TimeOnlyWrapper left, TimeOnlyWrapper right) => left.Inner <= right.Inner;
}