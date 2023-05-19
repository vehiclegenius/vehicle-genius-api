using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleGenius.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTemplatingToSummaryTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Template",
                table: "SummaryTemplates",
                newName: "DataTemplate");

            migrationBuilder.AddColumn<string>(
                name: "SystemPrompt",
                table: "SummaryTemplates",
                type: "text",
                nullable: false,
                defaultValue: "You are a helpful assistant.");

            migrationBuilder.AddColumn<string>(
                name: "PromptTemplate",
                table: "SummaryTemplates",
                type: "text",
                nullable: false,
                defaultValue: @"With this data:

{Data}

{UserMessage}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemPrompt",
                table: "SummaryTemplates");

            migrationBuilder.DropColumn(
                name: "PromptTemplate",
                table: "SummaryTemplates");

            migrationBuilder.RenameColumn(
                name: "DataTemplate",
                table: "SummaryTemplates",
                newName: "Template");
        }
    }
}
