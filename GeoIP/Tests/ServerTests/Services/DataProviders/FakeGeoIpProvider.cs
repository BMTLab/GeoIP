#region HEADER
//   FakeGeoIpProvider.cs of GeoIP.Tests
//   Created by Nikita Neverov at 21.01.2020 9:38
#endregion


using System.Net;
using System.Threading.Tasks;

using GeoIP.Server.Services.DataProviders;
using GeoIP.Shared.Models;


namespace GeoIP.Tests.ServerTests.Services.DataProviders
{
    public sealed class FakeGeoIpProvider : IGeoIpProvider
    {
        #region Fields
        internal readonly Block? TestBlock;
        #endregion


        #region Constructors
        public FakeGeoIpProvider()
        {
            TestBlock = new Block
            {
                GeonameId = 1,
                RegisteredCountryGeonameId = 1,
                RepresentedCountryGeonameId = 1,
                IsAnonymousProxy = false,
                IsSatelliteProvider = false,
                PostalCode = "076",
                Latitude = 34,
                Longitude = 35,
                AccuracyRadius = 50,
                Location = new Location
                {
                    GeonameId = 1,
                    LocaleCode = "en",
                    ContinentCode = "1",
                    ContinentName = "America",
                    CountryIsoCode = "01",
                    CountryName = "USA",
                    Subdivision1IsoCode = "1",
                    Subdivision1Name = "2",
                    Subdivision2IsoCode = "3",
                    Subdivision2Name = null,
                    CityName = "Chicago",
                    MetroCode = 80,
                    TimeZone = "-5",
                    IsInEuropeanUnion = false,
                    Blocks = null
                }
            };
        }
        #endregion


        #region Methods
        public Block? GetAllInfoByIp(string ip)
        {
            if (ip == "0.0.0.0")
                return null;

            TestBlock.Network = (IPAddress.Parse(ip), 24);

            return TestBlock;
        }


        public async Task<Block?> GetAllInfoByIpAsync(string ip) =>
            await Task.Run(() => GetAllInfoByIp(ip)).ConfigureAwait(false);
        #endregion
    }
}