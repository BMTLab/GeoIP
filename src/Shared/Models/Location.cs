using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using JetBrains.Annotations;


namespace Shared.Models;


[PublicAPI]
[SuppressMessage("Usage", "CA2227", MessageId = "Collection properties should be read only")]
public record Location
{
    public int GeonameId { get; init; }
    public string? LocaleCode { get; init; }
    public string? ContinentCode { get; init; }
    public string? ContinentName { get; init; }
    public string? CountryIsoCode { get; init; }
    public string? CountryName { get; init; }
    public string? Subdivision1IsoCode { get; init; }
    public string? Subdivision1Name { get; init; }
    public string? Subdivision2IsoCode { get; init; }
    public string? Subdivision2Name { get; init; }
    public string? CityName { get; init; }
    public short? MetroCode { get; init; }
    public string? TimeZone { get; init; }
    public bool? IsInEuropeanUnion { get; init; }

    [IgnoreDataMember]
    public virtual ICollection<Block>? Blocks { get; set; }
}