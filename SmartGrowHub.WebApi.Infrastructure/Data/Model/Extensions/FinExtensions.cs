namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

public static class FinExtensions
{
    public static TryAsync<T> ToTryAsync<T>(this Fin<Task<T>> fin) =>
        fin.Match(
            Succ: task => TryAsync(task),
            Fail: error => TryAsync<T>(error.ToException()));
}