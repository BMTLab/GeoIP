using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Shared.Services.Configuration.Extensions;


public static class ConfigurationOptionsExtensions
{
    public static void AddSectionOptions<T>(this IServiceCollection services, IConfiguration configuration) where T : class, new()
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));
            
        services.Configure<T>(configuration.GetSection(typeof(T).Name));
    }


        
    public static T GetFrom<T>(this IConfiguration configuration) where T : class, new()
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        return configuration.GetSection(typeof(T).Name).Get<T>();
    }
}