namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public interface IOperations<in T, out TResult>
{
    TResult Subtract(T other);
    bool IsGreaterThan(T other);
    bool IsLessThan(T other);
    bool IsGreaterThanOrEqual(T other);
    bool IsLessThanOrEqual(T other);
}