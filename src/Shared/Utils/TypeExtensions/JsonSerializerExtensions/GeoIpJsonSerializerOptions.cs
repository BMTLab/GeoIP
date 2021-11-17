using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shared.Utils.Converters.JsonConverters;


namespace Shared.Utils.TypeExtensions.JsonSerializerExtensions;

public static class GeoIpJsonSerializerOptions
{
    //static WsJsonSerializerOptions() {}
    
    
    private static readonly Lazy<JsonSerializerOptions> Saved =
        new
        (
            () => SetOptions(new JsonSerializerOptions()),
            LazyThreadSafetyMode.PublicationOnly
        );
        
    public static JsonSerializerOptions Instance => Saved.Value;

    
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static JsonSerializerOptions SetOptions(this JsonSerializerOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));
            
        #if !RELEASE
        options.WriteIndented = true;
        #else
        options.WriteIndented = false;
        #endif

        options.ReadCommentHandling = JsonCommentHandling.Skip;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        options.IgnoreReadOnlyFields = true;
        options.IgnoreReadOnlyProperties = true;
        options.DefaultBufferSize = 4;
        options.MaxDepth = 8;
        
        
        // Duplicate attribute values just in case
        options.Converters.Add(new BaseResultJsonConverter());

        return options;
    }
}