using System.Text.Json.Serialization;


namespace Shared.Utils.Converters.JsonConverters.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public sealed class JsonInterfaceConverterAttribute : JsonConverterAttribute
{
    public JsonInterfaceConverterAttribute(Type converterType)
        : base(converterType)
    {
    }
}