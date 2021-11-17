using Context;

using Server.Services.Providers.Abstractions;


namespace Server.Services.Providers.Ef.Abstractions;

public abstract class EfProvider<T> : Provider<T>
{
    protected readonly GeoIpDbContext Context;

    
    protected EfProvider
    (
        GeoIpDbContext context,
        ILogger<EfProvider<T>>? logger = null
    ) : base(logger)
    {
        Context = context;
    }
}