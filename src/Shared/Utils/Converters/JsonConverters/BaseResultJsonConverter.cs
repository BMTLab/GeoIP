using System.Text.Json;
using System.Text.Json.Serialization;

using Shared.ViewModels.Results.Abstractions;


namespace Shared.Utils.Converters.JsonConverters;

public sealed class BaseResultJsonConverter : JsonConverter<IBaseResult>
{
    public override IBaseResult Read
    (
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var typeFullName = root.GetProperty("typeName").GetString();
        var type = Type.GetType(typeFullName ?? throw new JsonException($"{nameof(IBaseResult.TypeName)} is null"), true, true);
        if (type == null)
            throw new JsonException($"Not found type with name {typeFullName}");
            
        return (IBaseResult) JsonSerializer.Deserialize(root.ToString(), type, options)!;
    }



    public override void Write(
        Utf8JsonWriter writer,
        IBaseResult value, 
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (IBaseResult) null!, options);
                break;
            default:
            {
                var type = value.GetType();
                JsonSerializer.Serialize(writer, value, type, options);
                break;
            }
        }
    }
}