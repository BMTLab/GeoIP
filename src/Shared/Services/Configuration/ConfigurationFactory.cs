using Microsoft.Extensions.Configuration;

using Shared.Utils.TypeExtensions;


namespace Shared.Services.Configuration;

public static class ConfigurationFactory
{
    public static readonly string PropertiesPath = Path.Combine(Directory.GetCurrentDirectory(), @"Properties");
    
    public static IConfigurationRoot CreateConfiguration(string[]? args, string environment)
    {
        var configBuilder = new ConfigurationBuilder()
                           .SetBasePath(PropertiesPath)
                           .AddJsonFile(@"appSettings.json", false, true);
        
        if (environment == "Development")
            configBuilder.AddJsonFile($@"appSettings.{environment}.json", false, true);

        if (args.IsValid())
            configBuilder.AddCommandLine(args);

        return configBuilder.Build();
    }
}