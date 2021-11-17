using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

using Server.Models.Constants;
using Server.Settings;

using Shared.Services.Configuration;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;


namespace Server;

public static class Program
{
    static Program()
    {
        #if RELEASE
        GeoIpEnvironment.AspNetCoreEnvironment = EnvironmentNames.Production;
        #else
        GeoIpEnvironment.AspNetCoreEnvironment = EnvironmentNames.Development;
        #endif
    }

    #region Methods
    public static async Task Main(string[] args)
    {
        var environment = GeoIpEnvironment.AspNetCoreEnvironment;
        var configuration = ConfigurationFactory.CreateConfiguration(args, environment);

        var nlogConfigPath = Path.Combine(ConfigurationFactory.PropertiesPath, @"NLog.config");
        var nLogger = LogManager
                     .Setup()
                     .LoadConfigurationFromFile(nlogConfigPath)
                     .RegisterNLogWeb(configuration)
                     .GetCurrentClassLogger();
        
        try
        {
            var host = CreateWebHost(args, configuration).Build();

            nLogger.Info("The app is preparing to start");
            
            await host.RunAsync();
        }
        catch (Exception exc)
        {
            nLogger.Fatal(exc, NeutralMessages.ServerFatal);

            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
    

    private static IHostBuilder CreateWebHost(string[] args, IConfiguration configuration) =>
        Host.CreateDefaultBuilder(args)
             #if _UNIX && !DEBUG
            .UseSystemd()
             #endif
            .ConfigureWebHostDefaults
             (
                 webBuilder =>
                     webBuilder
                        .UseEnvironment(GeoIpEnvironment.AspNetCoreEnvironment)
                        .UseConfiguration(configuration)
                         #if RELEASE
                        .UseShutdownTimeout(TimeSpan.FromSeconds(3))
                         #endif
                        //.UseStaticWebAssets()
                        .UseStartup<Startup>()
                        .UseKestrel
                         (
                             options =>
                             {
                                 #if _UNIX && !DEBUG
                                 options.ListenUnixSocket($"{GeoIpEnvironment.UnixRuntimeDirPath}/0.socket");
                                 #endif
                                 
                                 options.ListenLocalhost(int.Parse(configuration[@"Port"]));
                             }
                         )
                        .ConfigureLogging
                         (
                             logging =>
                             {
                                 logging.SetMinimumLevel(LogLevel.Trace);
                                 logging.ClearProviders();
                                 logging.AddConfiguration(configuration.GetSection("Logging"));

                                 #if DEBUG
                                 logging.AddDebug();
                                 #else
                                 //logging.AddSystemdConsole();
                                 #endif
                                 
                                 logging.AddNLog(configuration, new NLogAspNetCoreOptions
                                 {
                                     IncludeScopes = true,
                                     CaptureMessageProperties = true,
                                     CaptureMessageTemplates = true,
                                     ReplaceLoggerFactory = false,
                                     RemoveLoggerFactoryFilter = false,
                                     RegisterServiceProvider = false,
                                     ParseMessageTemplates = false
                                 });
                             }
                         )
             );
    #endregion
}