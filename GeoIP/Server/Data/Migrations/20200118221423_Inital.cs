#region HEADER
//   20200118221423_Inital.cs of GeoIP.Server
//   Created by Nikita Neverov at 19.01.2020 1:14
#endregion


using System;
using System.Net;

using Microsoft.EntityFrameworkCore.Migrations;


namespace GeoIP.Server.Data.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "locations",
                table => new
                {
                    geoname_id = table.Column<short>(),
                    locale_code = table.Column<string>(maxLength: 2),
                    continent_code = table.Column<string>(maxLength: 2),
                    continent_name = table.Column<string>(),
                    country_iso_code = table.Column<string>(maxLength: 2),
                    country_name = table.Column<string>(),
                    subdivision_1_iso_code = table.Column<string>(maxLength: 3),
                    subdivision_1_name = table.Column<string>(),
                    subdivision_2_iso_code = table.Column<string>(maxLength: 3),
                    subdivision_2_name = table.Column<string>(),
                    city_name = table.Column<string>(),
                    metro_code = table.Column<short>(nullable: true),
                    time_zone = table.Column<string>(),
                    is_in_european_union = table.Column<bool>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("locations_pkey", x => x.geoname_id); });

            migrationBuilder.CreateTable(
                "blocks",
                table => new
                {
                    network = table.Column<ValueTuple<IPAddress, int>>(),
                    geoname_id = table.Column<short>(),
                    registered_country_geoname_id = table.Column<short>(nullable: true),
                    represented_country_geoname_id = table.Column<short>(nullable: true),
                    is_anonymous_proxy = table.Column<bool>(nullable: true),
                    is_satellite_provider = table.Column<bool>(nullable: true),
                    postal_code = table.Column<string>(),
                    latitude = table.Column<decimal>("numeric(6,4)", nullable: true),
                    longitude = table.Column<decimal>("numeric(7,4)", nullable: true),
                    accuracy_radius = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("blocks_pkey", x => x.network);

                    table.ForeignKey(
                        "blocks_geoname_id_fkey",
                        x => x.geoname_id,
                        "locations",
                        "geoname_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_blocks_geoname_id",
                "blocks",
                "geoname_id");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blocks");

            migrationBuilder.DropTable(
                name: "locations");
        }
    }
}