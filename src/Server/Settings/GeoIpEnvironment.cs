using System.Reflection;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

using Server.Models.Constants;


namespace Server.Settings;


public static class GeoIpEnvironment
{
    internal const string UnixRuntimeDirPath = @$"/run/{UnixAppName}";
    private const string UnixAppName = @"geoip-server";
    
    internal static readonly string ServerName = Assembly.GetExecutingAssembly().GetName().Name!;
    internal static readonly string ServerVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
    internal static readonly string? ServerDescription = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

    
    internal static readonly StaticFileOptions StaticFileOptions = new()
    {
        OnPrepareResponse = ctx =>
        {
            var headers = ctx.Context.Response.GetTypedHeaders();
            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(2)
            };
        },
        ServeUnknownFileTypes = true,
        HttpsCompression = HttpsCompressionMode.Compress
    };

    public static string AspNetCoreEnvironment
    {
        get => Environment.GetEnvironmentVariable(EnvironmentNames.Key) ?? EnvironmentNames.Development;
        internal set => Environment.SetEnvironmentVariable(EnvironmentNames.Key, value);
    }
}