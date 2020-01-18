#region HEADER
//   Blocks.cs of GeoIP.Shared
//   Created by Nikita Neverov at 19.01.2020 0:12
#endregion


using System;
using System.Net;


namespace GeoIP.Shared.Models
{
    public class Blocks
    {
        public ValueTuple<IPAddress, int> Network { get; set; }
        public short? GeonameId { get; set; }
        public short? RegisteredCountryGeonameId { get; set; }
        public short? RepresentedCountryGeonameId { get; set; }
        public bool? IsAnonymousProxy { get; set; }
        public bool? IsSatelliteProvider { get; set; }
        public string PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public short? AccuracyRadius { get; set; }

        public virtual Locations Geoname { get; set; }
    }
}