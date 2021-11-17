using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using Localization.Attributes;
using Localization.Models;


namespace Localization.Helpers;

internal static class ReflectionHelpers
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    [RequiresUnreferencedCode("Calls GetLocalizationTypes")]
    internal static object? GetLocalizedType(string language) =>
        (
            from typeInfo in GetLocalizationTypes()
            where typeInfo.Language.Name.Equals(language, StringComparison.OrdinalIgnoreCase)
            select Activator.CreateInstance(typeInfo.Type))
       .FirstOrDefault();


    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    [RequiresUnreferencedCode("Calls DefinedTypes")]
    private static IEnumerable<LocalizationTypeInfo> GetLocalizationTypes() =>
        from t in Assembly.GetExecutingAssembly().DefinedTypes
        let attributes = t.GetCustomAttributes(typeof(LanguageAttribute), false)
        where attributes is { Length: > 0 }
        select new LocalizationTypeInfo
        {
            Type = t,
            Language = attributes
                      .Cast<LanguageAttribute>()
                      .FirstOrDefault()
        };
}