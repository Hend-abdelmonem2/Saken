using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saken_WebApplication.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class addsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotificationsEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThemeMode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotificationsEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ThemeMode",
                table: "AspNetUsers");
        }
    }
}
