namespace SmartGrowHub.Domain.Abstractions;

public interface IArithmetic<in T, out TResult>
{
    TResult Subtract(T other);
}