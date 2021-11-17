using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

using Shared.Models;


namespace Context;

public sealed class GeoIpDbContext : DbContext
{
    #region Constructors
    public GeoIpDbContext
    (
        DbContextOptions options
    ) : base(options)
    {
    }
    #endregion


    #region Properties.DbSets
    [PublicAPI]
    public DbSet<Block> Blocks { get; set; } = default!;
    
    [PublicAPI]
    public DbSet<Location> Locations { get; set; } = default!;
    #endregion


    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Block>(entity =>
        {
            entity.ToTable("blocks");

            entity.HasKey(e => e.Network)
                  .HasName("blocks_pkey");

            entity.Property(e => e.Network)
                  .HasColumnName("network");

            entity.Property(e => e.AccuracyRadius)
                  .HasColumnName("accuracy_radius");

            entity.Property(e => e.GeonameId)
                  .HasColumnName("geoname_id");

            entity.Property(e => e.IsAnonymousProxy)
                  .HasColumnName("is_anonymous_proxy");

            entity.Property(e => e.IsSatelliteProvider)
                  .HasColumnName("is_satellite_provider");

            entity.Property(e => e.Latitude)
                  .HasColumnName("latitude")
                  .HasColumnType("numeric(6,4)");

            entity.Property(e => e.Longitude)
                  .HasColumnName("longitude")
                  .HasColumnType("numeric(7,4)");

            entity.Property(e => e.PostalCode)
                  .HasColumnName("postal_code");

            entity.Property(e => e.RegisteredCountryGeonameId)
                  .HasColumnName("registered_country_geoname_id");

            entity.Property(e => e.RepresentedCountryGeonameId)
                  .HasColumnName("represented_country_geoname_id");

            entity.HasOne(d => d.Location)
                  .WithMany(p => p.Blocks)
                  .HasForeignKey(d => d.GeonameId)
                  .HasConstraintName("blocks_geoname_id_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("locations");

            entity.HasKey(e => e.GeonameId)
                  .HasName("locations_pkey");

            entity.Property(e => e.GeonameId)
                  .HasColumnName("geoname_id")
                  .ValueGeneratedNever();

            entity.Property(e => e.CityName)
                  .HasColumnName("city_name");

            entity.Property(e => e.ContinentCode)
                  .HasColumnName("continent_code")
                  .HasMaxLength(2);

            entity.Property(e => e.ContinentName)
                  .HasColumnName("continent_name");

            entity.Property(e => e.CountryIsoCode)
                  .HasColumnName("country_iso_code")
                  .HasMaxLength(2);

            entity.Property(e => e.CountryName)
                  .HasColumnName("country_name");

            entity.Property(e => e.IsInEuropeanUnion)
                  .HasColumnName("is_in_european_union");

            entity.Property(e => e.LocaleCode)
                  .HasColumnName("locale_code")
                  .HasMaxLength(2);

            entity.Property(e => e.MetroCode)
                  .HasColumnName("metro_code");

            entity.Property(e => e.Subdivision1IsoCode)
                  .HasColumnName("subdivision_1_iso_code")
                  .HasMaxLength(3);

            entity.Property(e => e.Subdivision1Name)
                  .HasColumnName("subdivision_1_name");

            entity.Property(e => e.Subdivision2IsoCode)
                  .HasColumnName("subdivision_2_iso_code")
                  .HasMaxLength(3);

            entity.Property(e => e.Subdivision2Name)
                  .HasColumnName("subdivision_2_name");

            entity.Property(e => e.TimeZone)
                  .HasColumnName("time_zone");
        });
    }
    #endregion
}