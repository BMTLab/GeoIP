using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Context.Migrations
{
    public partial class v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    geoname_id = table.Column<int>(type: "integer", nullable: false),
                    locale_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    continent_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    continent_name = table.Column<string>(type: "text", nullable: true),
                    country_iso_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    country_name = table.Column<string>(type: "text", nullable: true),
                    subdivision_1_iso_code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    subdivision_1_name = table.Column<string>(type: "text", nullable: true),
                    subdivision_2_iso_code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    subdivision_2_name = table.Column<string>(type: "text", nullable: true),
                    city_name = table.Column<string>(type: "text", nullable: true),
                    metro_code = table.Column<short>(type: "smallint", nullable: true),
                    time_zone = table.Column<string>(type: "text", nullable: true),
                    is_in_european_union = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("locations_pkey", x => x.geoname_id);
                });

            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    network = table.Column<ValueTuple<IPAddress, int>>(type: "cidr", nullable: false),
                    geoname_id = table.Column<int>(type: "integer", nullable: true),
                    registered_country_geoname_id = table.Column<int>(type: "integer", nullable: true),
                    represented_country_geoname_id = table.Column<int>(type: "integer", nullable: true),
                    is_anonymous_proxy = table.Column<bool>(type: "boolean", nullable: true),
                    is_satellite_provider = table.Column<bool>(type: "boolean", nullable: true),
                    postal_code = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<decimal>(type: "numeric(6,4)", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(7,4)", nullable: true),
                    accuracy_radius = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("blocks_pkey", x => x.network);
                    table.ForeignKey(
                        name: "blocks_geoname_id_fkey",
                        column: x => x.geoname_id,
                        principalTable: "locations",
                        principalColumn: "geoname_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_blocks_geoname_id",
                table: "blocks",
                column: "geoname_id");
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