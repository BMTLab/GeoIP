using System.Text.Json.Serialization;

using Shared.Utils.Converters.JsonConverters;
using Shared.Utils.Converters.JsonConverters.Attributes;


namespace Shared.ViewModels.Results.Abstractions;

[JsonInterfaceConverter(typeof(BaseResultJsonConverter))]
public interface IBaseResult
{
    [JsonInclude]
    bool IsSuccessful { get; }
        
    [JsonInclude]
    [JsonPropertyName(nameof(TypeName))]
    string? TypeName { get; }
}