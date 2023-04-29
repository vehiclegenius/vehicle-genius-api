using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleGenius.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVinAuditDataVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VinAuditDataVersion",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VinAuditDataVersion",
                table: "SummaryTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VinAuditDataVersion",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VinAuditDataVersion",
                table: "SummaryTemplates");
        }
    }
}
