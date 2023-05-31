using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleGenius.Api.Migrations
{
    /// <inheritdoc />
    public partial class MapUserVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserVehicles_UserId",
                table: "UserVehicles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserVehicles",
                table: "UserVehicles",
                columns: new[] { "UserId", "VehicleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserVehicles",
                table: "UserVehicles");

            migrationBuilder.CreateIndex(
                name: "IX_UserVehicles_UserId",
                table: "UserVehicles",
                column: "UserId");
        }
    }
}
