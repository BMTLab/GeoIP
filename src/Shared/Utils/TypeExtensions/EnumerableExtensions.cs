using System.Diagnostics.CodeAnalysis;


namespace Shared.Utils.TypeExtensions;

public static class EnumerableExtensions
{
    /// <remarks>Extension methods.</remarks>
    /// <summary>
    ///     Checks IEnumerable collection for null, items and null items
    /// </summary>
    /// <remarks>Additionally, you can specify a validation <see cref="predicate" /></remarks>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    /// <returns>bool</returns>
    public static bool IsValid<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable, Predicate<IEnumerable<T>>? predicate = null)
    {
        if (enumerable is null)
            return false;

        var array = enumerable as T[] ?? enumerable.ToArray();

        if (!array.Any())
            return false;

        return predicate is null || predicate(array);
    }
}