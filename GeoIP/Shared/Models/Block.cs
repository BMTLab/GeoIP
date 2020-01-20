#region HEADER
//   Blocks.cs of GeoIP.Shared
//   Created by Nikita Neverov at 19.01.2020 0:12
#endregion


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Runtime.Serialization;


namespace GeoIP.Shared.Models
{
    [Serializable]
    public class Block
    {
        [Key]
        [IgnoreDataMember]
        public ValueTuple<IPAddress, int> Network { get; set; }

        public int? GeonameId { get; set; }
        public int? RegisteredCountryGeonameId { get; set; }
        public int? RepresentedCountryGeonameId { get; set; }
        public bool? IsAnonymousProxy { get; set; }
        public bool? IsSatelliteProvider { get; set; }
        public string? PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public short? AccuracyRadius { get; set; }

        [ForeignKey("GeonameId")]
        public virtual Location? Location { get; set; }
    }
}