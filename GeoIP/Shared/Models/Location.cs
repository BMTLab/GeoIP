#region HEADER
//   Locations.cs of GeoIP.Shared
//   Created by Nikita Neverov at 19.01.2020 17:02
#endregion


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace GeoIP.Shared.Models
{
    [Serializable]
    public class Location
    {
        [Key]
        public int GeonameId { get; set; }
        public string? LocaleCode { get; set; }
        public string? ContinentCode { get; set; }
        public string? ContinentName { get; set; }
        public string? CountryIsoCode { get; set; }
        public string? CountryName { get; set; }
        public string? Subdivision1IsoCode { get; set; }
        public string? Subdivision1Name { get; set; }
        public string? Subdivision2IsoCode { get; set; }
        public string? Subdivision2Name { get; set; }
        public string? CityName { get; set; }
        public short? MetroCode { get; set; }
        public string? TimeZone { get; set; }
        public bool? IsInEuropeanUnion { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Block>? Blocks { get; set; }
    }
}