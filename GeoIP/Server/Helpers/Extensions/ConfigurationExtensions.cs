#region HEADER
//   ConfigurationExtensions.cs of GeoIP.Server
//   Created by Nikita Neverov at 20.01.2020 12:40
#endregion


using Microsoft.Extensions.Configuration;


namespace GeoIP.Server.Helpers.Extensions
{
    public static class ConfigurationExtensions
    {
        #region Methods
        public static T GetCacheDurationByKey<T>
        (
            this IConfiguration configuration,
            string key,
            T defaultValue
        ) where T : struct =>
            configuration?.GetSection("CacheStorageDurations").GetValue(key, defaultValue) ?? default;
        #endregion
    }
}