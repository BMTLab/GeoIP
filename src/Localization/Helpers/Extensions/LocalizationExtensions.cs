using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;


namespace Localization.Helpers.Extensions;

/// <summary>
///     A class that contains a method for adding dynamic localization support to a ServiceCollection.
/// </summary>
[PublicAPI]
public static class LocalizationExtensions
{
    /// <summary>
    ///     Adds a Scoped ILocalization implementation to a ServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicLocalization
    (
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        LocalizationOptions? settings = null
    )
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (settings != null)
            LocalizationBuilder.Options = settings;

        services.AddSingleton<ILocalizationBuilder, LocalizationBuilder>();
        services.Add(new ServiceDescriptor(typeof(ILocalization), provider => provider.GetRequiredService<ILocalizationBuilder>().Localization, lifetime));

        return services;
    }


    /// <summary>
    ///     Adds a Scoped ILocalization implementation to a ServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicLocalization<T>
    (
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        LocalizationOptions? settings = null
    ) where T : class, ILocalizationBuilder
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        if (settings != null)
            LocalizationBuilder.Options = settings;

        services.AddSingleton<ILocalizationBuilder, T>();
        services.Add(new ServiceDescriptor(typeof(ILocalization), provider => provider.GetRequiredService<ILocalizationBuilder>().Localization, lifetime));

        return services;
    }
}