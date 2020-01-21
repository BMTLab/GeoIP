#region HEADER
//   Program.cs of GeoIP.Client
//   Created by Nikita Neverov at 19.01.2020 1:43
#endregion


using Microsoft.AspNetCore.Blazor.Hosting;


namespace GeoIP.Client
{
    public static class Program
    {
        public static void Main() => CreateHostBuilder().Build().Run();


        private static IWebAssemblyHostBuilder CreateHostBuilder() =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                                 .UseBlazorStartup<Startup>();
    }
}