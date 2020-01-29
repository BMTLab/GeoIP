#region HEADER
//   ServiceProviderExtensions.cs of GeoIP.Server
//   Created by Nikita Neverov at 20.01.2020 12:51
#endregion


using GeoIP.Server.Services.DataProviders;

using Microsoft.Extensions.DependencyInjection;


namespace GeoIP.Server.Services.Extensions
{
    public static class ServiceProviderExtensions
    {
        #region Methods
        public static IServiceCollection AddGeoIpProvider(this IServiceCollection services) =>
            services.AddScoped<IGeoIpProvider, GeoIpProvider>();
        #endregion
    }
}