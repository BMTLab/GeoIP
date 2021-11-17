using System.Reflection;

using JetBrains.Annotations;


namespace Context.Utils;


[UsedImplicitly]
public static class ScriptResourceReader
{
    /// <summary>
    ///     Read embedded resource text file
    /// </summary>
    /// <param name="name">Format: "{Namespace}.{Folder}.{Filename}.{Extension}"</param>
    /// <returns>Script from file</returns>
    /// <exception cref="InvalidOperationException">Failed to read assembly manifest</exception>
    public static string ReadResource<T>(string name)
    {
        var assembly = 
            Assembly.GetAssembly(typeof(T))
            ?? throw new TypeLoadException($"Could not load assembly for specified type {typeof(T).Name}");
        
        var resourcePath = name;
        resourcePath = assembly.GetManifestResourceNames()
                               .Single(str => str.EndsWith(name));

        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Failed to read assembly manifest"));

        return reader.ReadToEnd();
    }
}