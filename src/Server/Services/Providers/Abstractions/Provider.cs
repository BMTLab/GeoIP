namespace Server.Services.Providers.Abstractions;

public abstract class Provider<T>
{
    protected readonly ILogger<Provider<T>>? Logger;
    
    protected Provider
    (
        ILogger<Provider<T>>? logger = null
    ) =>
        Logger = logger;
}