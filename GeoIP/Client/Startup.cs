#region HEADER
//   Startup.cs of GeoIP.Client
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace GeoIP.Client
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }


        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}