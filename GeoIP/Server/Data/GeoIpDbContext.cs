#region HEADER
//   GeoIpDbContext.cs of GeoIP.Server
//   Created by Nikita Neverov at 19.01.2020 1:14
#endregion


#define SENSITIVE_DATA_LOGGING

using System;
using System.IO;

using GeoIP.Shared.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace GeoIP.Server.Data
{
    public sealed class GeoIpDbContext : DbContext
    {
        #region Constructors
        public GeoIpDbContext()
        {
        }
        
        public GeoIpDbContext
        (
            DbContextOptions options
        ) : base(options)
        {
        }
        #endregion


        #region Properties.DbSets
        public DbSet<Blocks>? Blocks { get; set; }
        public DbSet<Locations>? Locations { get; set; }
        #endregion


        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder is null || optionsBuilder.IsConfigured)
                return;
            
            var connection = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile(@"Properties/appSettings.json")
                            .Build()
                            .GetConnectionString(@"Default");
            
            optionsBuilder.UseNpgsql(connection);

            #if DEBUG || SENSITIVE_DATA_LOGGING
            optionsBuilder.EnableSensitiveDataLogging();
            #endif
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder), "EF Core error");
            
            modelBuilder.Entity<Blocks>(entity =>
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
                      .WithMany(p => p!.Blocks)
                      .HasForeignKey(d => d.GeonameId)
                      .HasConstraintName("blocks_geoname_id_fkey");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.ToTable("locations");
                
                entity.HasKey(e => e.GeonameId)
                      .HasName("locations_pkey");

                entity.Property(e => e.GeonameId)
                      .HasColumnName("geoname_id")
                      .ValueGeneratedNever();

                entity.Property(e => e.CityName).HasColumnName("city_name");

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
}