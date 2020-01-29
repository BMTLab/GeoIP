#region HEADER
//   Program.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using System;
using System.IO;
using System.Threading.Tasks;

using Fody;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Web;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;


namespace GeoIP.Server
{
    [ConfigureAwait(false)]
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog(@"Properties/NLog.config").GetCurrentClassLogger();
            var host = CreateWebHost(args);

            AppDomain.CurrentDomain.UnhandledException += (_, e) => logger.Error(e.ExceptionObject);

            try
            {
                await host.Build()
                          .RunAsync()
                          .ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                logger.Fatal(exc);
            }
            finally
            {
                LogManager.Shutdown();
            }
        }


        private static IWebHostBuilder CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseConfiguration(new ConfigurationBuilder()
                                    .AddCommandLine(args)
                                    .Build())
                   .ConfigureAppConfiguration((_, config) =>
                    {
                        var path = string.Concat(Directory.GetCurrentDirectory(), @"/Properties/appSettings.json");
                        config.AddJsonFile(path, false, true);
                    })
                   .UseStartup<Startup>()
                   .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                   .UseNLog();
    }
}