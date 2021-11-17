using JetBrains.Annotations;


namespace Localization.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class LanguageAttribute : Attribute
{
    internal LanguageAttribute(string name)
    {
        if (name is null || string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), @"The name of the language should not be empty");

        Name = name;
    }


    [PublicAPI]
    internal string Name { get; }
}