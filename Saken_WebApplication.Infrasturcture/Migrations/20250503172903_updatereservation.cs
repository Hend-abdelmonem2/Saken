using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saken_WebApplication.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class updatereservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_UserId",
                table: "reservations");

            migrationBuilder.AddColumn<string>(
                name: "LandlordId",
                table: "reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandlordId1",
                table: "reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_LandlordId",
                table: "reservations",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_LandlordId1",
                table: "reservations",
                column: "LandlordId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_LandlordId",
                table: "reservations",
                column: "LandlordId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_LandlordId1",
                table: "reservations",
                column: "LandlordId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_UserId",
                table: "reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_LandlordId",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_LandlordId1",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_UserId",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "IX_reservations_LandlordId",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "IX_reservations_LandlordId1",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "LandlordId",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "LandlordId1",
                table: "reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_UserId",
                table: "reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
