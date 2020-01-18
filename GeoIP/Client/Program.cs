#region HEADER
//   Program.cs of GeoIP.Client
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using Microsoft.AspNetCore.Blazor.Hosting;


namespace GeoIP.Client
{
    public static class Program
    {
        public static void Main(string[] args) => 
            CreateHostBuilder(args).Build().Run();


        private static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                                 .UseBlazorStartup<Startup>();
    }
}