#region HEADER
//   Program.cs of GeoIP.Client
//   Created by Nikita Neverov at 19.01.2020 1:43
#endregion


using System.Threading.Tasks;

using Microsoft.AspNetCore.Blazor.Hosting;


namespace GeoIP.Client
{
    public class Program
    {
        public static async Task Main()
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>(@"app");

            await builder.Build().RunAsync();
        }
    }
}