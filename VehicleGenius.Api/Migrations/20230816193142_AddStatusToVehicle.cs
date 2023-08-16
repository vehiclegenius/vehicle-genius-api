using Microsoft.EntityFrameworkCore.Migrations;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.DIMO;
using VehicleGenius.Api.Services.Vehicles.VinAudit;

#nullable disable

namespace VehicleGenius.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<VinAuditData>(
                name: "VinAuditData",
                table: "Vehicles",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(VinAuditData),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<VehicleUserDataDto>(
                name: "UserData",
                table: "Vehicles",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(VehicleUserDataDto),
                oldType: "jsonb");

            migrationBuilder.AddColumn<DimoVehicleStatus>(
                name: "DimoVehicleStatus",
                table: "Vehicles",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DimoVehicleStatus",
                table: "Vehicles");

            migrationBuilder.AlterColumn<VinAuditData>(
                name: "VinAuditData",
                table: "Vehicles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(VinAuditData),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<VehicleUserDataDto>(
                name: "UserData",
                table: "Vehicles",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(VehicleUserDataDto),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
