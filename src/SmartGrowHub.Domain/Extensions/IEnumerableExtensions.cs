using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<(T First, T Second)> GetUniquePairs<T>(this IEnumerable<T> enumerable)
    {
        ImmutableArray<T> array = [.. enumerable];
        
        for (var i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                yield return (array[i], array[j]);
            }
        }
    }
}