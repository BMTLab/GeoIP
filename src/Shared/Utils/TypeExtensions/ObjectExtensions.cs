namespace Shared.Utils.TypeExtensions;


public static class ObjectExtensions
{
    /// <remarks>Extension method.</remarks>
    /// <summary>
    ///     Gets all properties into <see cref="instance" /> specified by type <see cref="TE" />.
    /// </summary>
    /// <param name="instance">Class <see cref="T" /> object whose initialized properties returns</param>
    /// <param name="capacity">The number of expected properties, the default is 4</param>
    /// <typeparam name="T">Variable type <see cref="instance" /></typeparam>
    /// <typeparam name="TE">Property type of interest</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <see cref="instance" /> is not initialized</exception>
    /// <returns>
    ///     Returns an IEnumerable/<TE /> collection of tuples,
    ///     where type TE is the property type, and string name is the name of the property
    /// </returns>
    /// <remarks>Will return an empty collection if no matches are found</remarks>
    public static IEnumerable<(TE type, string name)> GetTypedProperties<T, TE>(this T instance, byte capacity = 4)
        where T : class
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        var collection = new Stack<(TE, string)>(capacity);

        foreach (var field in typeof(T).GetProperties())
            if (field.GetValue(instance, null) is TE expected)
                collection.Push((expected, field.Name));

        return collection;
    }
}