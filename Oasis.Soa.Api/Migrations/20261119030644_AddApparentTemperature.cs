using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oasis.Soa.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddApparentTemperature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ApparentTemperatureC",
                table: "WeatherReadings",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApparentTemperatureC",
                table: "WeatherReadings");
        }
    }
}
