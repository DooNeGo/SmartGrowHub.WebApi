namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public readonly record struct TimeOnlyWrapper(TimeOnly InnerTimeOnly) : IOperations<TimeOnlyWrapper, TimeSpan>
{
    public TimeSpan Subtract(TimeOnlyWrapper other) => InnerTimeOnly - other.InnerTimeOnly;

    public bool IsGreaterThan(TimeOnlyWrapper other) => InnerTimeOnly > other.InnerTimeOnly;

    public bool IsLessThan(TimeOnlyWrapper other) => InnerTimeOnly < other.InnerTimeOnly;

    public bool IsGreaterThanOrEqual(TimeOnlyWrapper other) => InnerTimeOnly >= other.InnerTimeOnly;

    public bool IsLessThanOrEqual(TimeOnlyWrapper other) => InnerTimeOnly <= other.InnerTimeOnly;
}