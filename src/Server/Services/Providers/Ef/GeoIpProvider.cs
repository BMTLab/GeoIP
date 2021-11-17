using Context;

using Microsoft.EntityFrameworkCore;

using Server.Services.Providers.Ef.Abstractions;

using Shared.Models;


namespace Server.Services.Providers.Ef;


public sealed class GeoIpProvider : IGeoIpProvider
{
    #region Fields
    private static readonly Func<GeoIpDbContext, string, Task<Block?>> GetByIpFunc =
        (db, ip) =>
            db.Blocks
              .FromSqlRaw($"select * from geoipdb.public.blocks where network >> '{ip}' limit 1")
              .AsNoTracking()
              .Include(p => p.Location)
              .FirstOrDefaultAsync();
      
    
    
    private readonly GeoIpDbContext _db;
    private readonly ILogger<GeoIpProvider>? _logger;
    #endregion

    
    public GeoIpProvider
    (
        GeoIpDbContext context,
        ILogger<GeoIpProvider>? logger = null
    )
    {
        _db = context;
        _logger = logger;
    }


    #region Methods
    /// <summary>
    ///     Returns all database fields for this ip
    /// </summary>
    public async Task<Block?> GetByIpAsync(string ip) =>
        await GetByIpFunc(_db, ip);
    #endregion _Methods
}