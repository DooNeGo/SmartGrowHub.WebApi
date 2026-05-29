using System.Numerics;

namespace SmartGrowHub.Domain.Common;

public readonly record struct TimedQuantity<TTime>(Quantity Quantity, TimeInterval<TTime> TimeInterval)
    where TTime : IComparisonOperators<TTime, TTime, bool>, ISubtractionOperators<TTime, TTime, TimeSpan>;