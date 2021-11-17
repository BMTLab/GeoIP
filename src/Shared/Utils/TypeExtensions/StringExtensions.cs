using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using static System.String;


namespace Shared.Utils.TypeExtensions;


public static class StringExtensions
{
    /// <remarks>Extension method.</remarks>
    /// <summary>
    ///     Extension method for string.Equal. Comparison ignores case and culture
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns>bool</returns>
        
    public static bool CompareIgnoreCase([NotNullWhen(false)] this string? s1, [NotNullWhen(false)] in string? s2)
    {
        return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
    }


    /// <remarks>Extension method.</remarks>
    /// <summary>
    ///     Checks the string for null and empty space.
    /// </summary>
    /// <remarks>Additionally, you can specify a validation <see cref="predicate" /></remarks>
    /// <param name="s"></param>
    /// <param name="predicate"></param>
    /// <returns>bool</returns>
        
    public static bool IsValid([NotNullWhen(true)] this string? s, Predicate<string>? predicate = null)
    {
        if (IsNullOrWhiteSpace(s))
            return false;

        return predicate is null || predicate(s);
    }


    /// <summary>
    ///     Throws an <see cref="ArgumentNullException" /> if the input string is null or empty, otherwise returns the original
    ///     string
    /// </summary>
    /// <param name="forСheck">Input string</param>
    /// <param name="predicate">Custom optional condition</param>
    /// <param name="name">Name of property to show error</param>
    /// <returns><see cref="forСheck" />Input string <see cref="forСheck" /></returns>
    /// <exception cref="ArgumentNullException">Exception thrown when validation fails</exception>
        
    public static string EnsureValid(this string? forСheck, Predicate<string>? predicate = null, [CallerMemberName] string name = "")
    {
        if (!forСheck.IsValid(predicate))
            throw new ArgumentNullException(name, "The parameter did not pass the check for a non-empty value");

        return forСheck;
    }
}