using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saken_WebApplication.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class updateResrvationmod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_LandlordId1",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "IX_reservations_LandlordId1",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "LandlordId1",
                table: "reservations");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "reservations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservations_UserId1",
                table: "reservations",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_UserId1",
                table: "reservations",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_UserId1",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "IX_reservations_UserId1",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "reservations");

            migrationBuilder.AddColumn<string>(
                name: "LandlordId1",
                table: "reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_LandlordId1",
                table: "reservations",
                column: "LandlordId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_LandlordId1",
                table: "reservations",
                column: "LandlordId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
