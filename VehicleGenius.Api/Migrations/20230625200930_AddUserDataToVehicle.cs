using Microsoft.EntityFrameworkCore.Migrations;
using VehicleGenius.Api.Dtos;

#nullable disable

namespace VehicleGenius.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDataToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<VehicleUserDataDto>(
                name: "UserData",
                table: "Vehicles",
                type: "jsonb",
                nullable: false,
                defaultValue: new object());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserData",
                table: "Vehicles");
        }
    }
}
