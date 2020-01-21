#region HEADER
//   GeoIpProvider.cs of GeoIP.Server
//   Created by Nikita Neverov at 20.01.2020 10:02
#endregion


using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using GeoIP.Server.Data;
using GeoIP.Server.Helpers.Extensions;
using GeoIP.Shared.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace GeoIP.Server.Services.DataProviders
{
    public sealed class GeoIpProvider : IGeoIpProvider
    {
        #pragma warning disable IDE0052
        
        #region Fields
        private static readonly Func<GeoIpDbContext, string, Block?> GetAllInfoByIpFunc =
            (db, ip) => db?.Blocks
                          .FromSqlRaw($"select * from geoipdb.public.blocks where '{ip}' <<= network")
                          .Include(p => p.Location)
                          .AsNoTracking()
                          .FirstOrDefault();

        private readonly GeoIpDbContext _db;
        private readonly IMemoryCache? _cache;
        private readonly ILogger<GeoIpProvider>? _logger;
        private readonly TimeSpan _dbCacheStorageDuration;
        #endregion


        #region Constructors
        public GeoIpProvider
        (
            GeoIpDbContext context,
            IMemoryCache? memoryCache = null,
            IConfiguration? configuration = null,
            ILogger<GeoIpProvider>? logger = null
        )
        {
            _db = context;
            _cache = memoryCache;
            _logger = logger;

            _dbCacheStorageDuration = TimeSpan
               .FromMinutes(configuration?.GetCacheDurationByKey("DbCacheDurationMinutes", 5) 
                            ?? 5);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Returns all database fields for this ip
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public Block? GetAllInfoByIp(string ip)
        {
            if (_cache.TryGetValue(ip, out Block? block))
            {
                _logger?.LogTrace("IP request loaded from cache");

                goto Out;
            }

            block = GetAllInfoByIpFunc(_db, ip);

            if (block != null)
            {
                _cache?.Set(ip, block, new MemoryCacheEntryOptions().SetAbsoluteExpiration(_dbCacheStorageDuration));
                _logger?.LogTrace("IP request saved to cache");
            }

            Out:

            return block;
        }
        
        /// <summary>
        /// Asynchronous version of 'GetAllInfoByIp'
        /// </summary>
        public async Task<Block?> GetAllInfoByIpAsync(string ip) =>
            await Task.Run(() => GetAllInfoByIp(ip)).ConfigureAwait(false);
        #endregion _Methods


        #pragma warning restore CA1823
    }
}