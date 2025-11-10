using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saken_WebApplication.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rate",
                table: "AspNetUsers",
                newName: "UserRatingAverage");

            migrationBuilder.AddColumn<string>(
                name: "UserId2",
                table: "reservations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRatingCount",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contacts_AspNetUsers_ContactUserId",
                        column: x => x.ContactUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contacts_AspNetUsers_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "housingOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HousingId = table.Column<int>(type: "int", nullable: false),
                    OfferType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountedPricePerMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountedInsuranceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsCommissionFree = table.Column<bool>(type: "bit", nullable: true),
                    IsFirstMonthFree = table.Column<bool>(type: "bit", nullable: true),
                    IncludesFreeInternet = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_housingOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_housingOffer_houses_HousingId",
                        column: x => x.HousingId,
                        principalTable: "houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservations_UserId2",
                table: "reservations",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_ContactUserId",
                table: "contacts",
                column: "ContactUserId");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_OwnerUserId",
                table: "contacts",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_housingOffer_HousingId",
                table: "housingOffer",
                column: "HousingId");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_AspNetUsers_UserId2",
                table: "reservations",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_AspNetUsers_UserId2",
                table: "reservations");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "housingOffer");

            migrationBuilder.DropIndex(
                name: "IX_reservations_UserId2",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "UserRatingCount",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserRatingAverage",
                table: "AspNetUsers",
                newName: "rate");
        }
    }
}
