using System.Reflection;

using Localization.Attributes;

namespace Localization.Models;


#nullable disable
internal sealed class LocalizationTypeInfo
{
    public TypeInfo Type { get; set; }
    public LanguageAttribute Language { get; set; }
}