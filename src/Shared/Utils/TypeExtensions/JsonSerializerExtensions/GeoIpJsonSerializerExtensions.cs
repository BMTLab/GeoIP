using System.Text.Json;


namespace Shared.Utils.TypeExtensions.JsonSerializerExtensions;

public static class GeoIpJsonSerializerExtensions
{
    public static string Serialize<T>(this T model) =>
        JsonSerializer.Serialize(model, GeoIpJsonSerializerOptions.Instance);
    
    public static T Deserialize<T>(this string json) =>
        JsonSerializer.Deserialize<T>(json, GeoIpJsonSerializerOptions.Instance) ??
        throw new InvalidCastException($"Unable to deserialize json: {json} into object {typeof(T)}");
    

    public static async Task<IAsyncEnumerable<T?>> DeserializeAsyncEnumerable<T>(this Task<Stream> streamTask) =>
        JsonSerializer.DeserializeAsyncEnumerable<T>(await streamTask, GeoIpJsonSerializerOptions.Instance) ??
        throw new InvalidCastException($"Unable to deserialize stream into async enumerable object {typeof(T)}");
        
        
    public static async Task<T> DeserializeAsync<T>(this Task<Stream> streamTask) =>
        await JsonSerializer.DeserializeAsync<T>(await streamTask, GeoIpJsonSerializerOptions.Instance).ConfigureAwait(false) ??
        throw new InvalidCastException($"Unable to deserialize stream into object {typeof(T)}");
}