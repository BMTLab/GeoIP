using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Runtime.Serialization;

using JetBrains.Annotations;


namespace Shared.Models;


[PublicAPI]
public record Block
{
    public ValueTuple<IPAddress, int> Network { get; init; }
    public int? GeonameId { get; init; }
    public int? RegisteredCountryGeonameId { get; init; }
    public int? RepresentedCountryGeonameId { get; init; }
    public bool? IsAnonymousProxy { get; init; }
    public bool? IsSatelliteProvider { get; init; }
    public string? PostalCode { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public short? AccuracyRadius { get; init; }

    [ForeignKey("GeonameId")]
    [IgnoreDataMember]
    public virtual Location? Location { get; set; }
}