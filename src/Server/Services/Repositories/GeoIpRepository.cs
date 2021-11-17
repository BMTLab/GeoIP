using Microsoft.Extensions.Caching.Memory;

using OneOf;

using Server.Services.Providers.Ef.Abstractions;
using Server.Services.Repositories.Abstractions;
using Server.Services.Repositories.StateModels;

using Shared.Models;


namespace Server.Services.Repositories;

public class GeoIpRepository : BaseRepository, IGeoIpRepository
{
    private readonly IGeoIpProvider _provider;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _dbCacheStorageDuration;


    public GeoIpRepository
    (
        IGeoIpProvider provider,
        IMemoryCache memoryCache,
        ILogger<GeoIpRepository>? logger = null
    ) : base(logger)
    {
        _cache = memoryCache;
        _provider = provider;
        _cache = memoryCache;

        _dbCacheStorageDuration = TimeSpan.FromMinutes(5);
    }


    #region Methods
    /// <summary>
    ///     Returns all database fields for this ip
    /// </summary>
    public async Task<OneOf<Block, CommonStates.NotFound>> GetByIpAsync(string ip)
    {
        if (_cache.TryGetValue(ip, out Block existBlock))
        {
            Logger?.LogDebug($"Received from cache: {ip}");
            return existBlock;
        }

        var block = await _provider.GetByIpAsync(ip);

        if (block is null)
            return new CommonStates.NotFound();

        _cache.Set(ip, block, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_dbCacheStorageDuration));
        
        return block;
    }
    #endregion _Methods
}