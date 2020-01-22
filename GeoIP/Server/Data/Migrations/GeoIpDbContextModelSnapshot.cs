#region HEADER
//   GeoIpDbContextModelSnapshot.cs of GeoIP.Server
//   Created by Nikita Neverov at 21.01.2020 22:36
#endregion


using System;
using System.Net;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;


namespace GeoIP.Server.Data.Migrations
{
    [DbContext(typeof(GeoIpDbContext))]
    internal class GeoIpDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            #pragma warning disable 612, 618
            modelBuilder
               .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
               .HasAnnotation("ProductVersion", "3.1.1")
               .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GeoIP.Shared.Models.Block", b =>
            {
                b.Property<ValueTuple<IPAddress, int>>("Network")
                 .HasColumnName("network")
                 .HasColumnType("cidr");

                b.Property<short?>("AccuracyRadius")
                 .HasColumnName("accuracy_radius")
                 .HasColumnType("smallint");

                b.Property<int?>("GeonameId")
                 .HasColumnName("geoname_id")
                 .HasColumnType("integer");

                b.Property<bool?>("IsAnonymousProxy")
                 .HasColumnName("is_anonymous_proxy")
                 .HasColumnType("boolean");

                b.Property<bool?>("IsSatelliteProvider")
                 .HasColumnName("is_satellite_provider")
                 .HasColumnType("boolean");

                b.Property<decimal?>("Latitude")
                 .HasColumnName("latitude")
                 .HasColumnType("numeric(6,4)");

                b.Property<decimal?>("Longitude")
                 .HasColumnName("longitude")
                 .HasColumnType("numeric(7,4)");

                b.Property<string>("PostalCode")
                 .HasColumnName("postal_code")
                 .HasColumnType("text");

                b.Property<int?>("RegisteredCountryGeonameId")
                 .HasColumnName("registered_country_geoname_id")
                 .HasColumnType("integer");

                b.Property<int?>("RepresentedCountryGeonameId")
                 .HasColumnName("represented_country_geoname_id")
                 .HasColumnType("integer");

                b.HasKey("Network")
                 .HasName("blocks_pkey");

                b.HasIndex("GeonameId");

                b.ToTable("blocks");
            });

            modelBuilder.Entity("GeoIP.Shared.Models.Location", b =>
            {
                b.Property<int?>("GeonameId")
                 .HasColumnName("geoname_id")
                 .HasColumnType("integer");

                b.Property<string>("CityName")
                 .HasColumnName("city_name")
                 .HasColumnType("text");

                b.Property<string>("ContinentCode")
                 .HasColumnName("continent_code")
                 .HasColumnType("character varying(2)")
                 .HasMaxLength(2);

                b.Property<string>("ContinentName")
                 .HasColumnName("continent_name")
                 .HasColumnType("text");

                b.Property<string>("CountryIsoCode")
                 .HasColumnName("country_iso_code")
                 .HasColumnType("character varying(2)")
                 .HasMaxLength(2);

                b.Property<string>("CountryName")
                 .HasColumnName("country_name")
                 .HasColumnType("text");

                b.Property<bool?>("IsInEuropeanUnion")
                 .HasColumnName("is_in_european_union")
                 .HasColumnType("boolean");

                b.Property<string>("LocaleCode")
                 .HasColumnName("locale_code")
                 .HasColumnType("character varying(2)")
                 .HasMaxLength(2);

                b.Property<short?>("MetroCode")
                 .HasColumnName("metro_code")
                 .HasColumnType("smallint");

                b.Property<string>("Subdivision1IsoCode")
                 .HasColumnName("subdivision_1_iso_code")
                 .HasColumnType("character varying(3)")
                 .HasMaxLength(3);

                b.Property<string>("Subdivision1Name")
                 .HasColumnName("subdivision_1_name")
                 .HasColumnType("text");

                b.Property<string>("Subdivision2IsoCode")
                 .HasColumnName("subdivision_2_iso_code")
                 .HasColumnType("character varying(3)")
                 .HasMaxLength(3);

                b.Property<string>("Subdivision2Name")
                 .HasColumnName("subdivision_2_name")
                 .HasColumnType("text");

                b.Property<string>("TimeZone")
                 .HasColumnName("time_zone")
                 .HasColumnType("text");

                b.HasKey("GeonameId")
                 .HasName("locations_pkey");

                b.ToTable("locations");
            });

            modelBuilder.Entity("GeoIP.Shared.Models.Block", b =>
            {
                b.HasOne("GeoIP.Shared.Models.Location", "Location")
                 .WithMany("Blocks")
                 .HasForeignKey("GeonameId")
                 .HasConstraintName("blocks_geoname_id_fkey");
            });
            #pragma warning restore 612, 618
        }
    }
}