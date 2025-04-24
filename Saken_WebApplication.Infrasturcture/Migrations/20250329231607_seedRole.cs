using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saken_WebApplication.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class seedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "User", "User".ToUpper(), Guid.NewGuid().ToString() }
               );
            migrationBuilder.InsertData(
           table: "AspNetRoles",
           columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
           values: new object[] { Guid.NewGuid().ToString(), "Admin ", "Admin".ToUpper(), Guid.NewGuid().ToString() }
           );
           migrationBuilder.InsertData(
          table: "AspNetRoles",
          columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
          
          values: new object[] { Guid.NewGuid().ToString(), "Owner", "Owner".ToUpper(), Guid.NewGuid().ToString() }
          );
          migrationBuilder.InsertData(
         table: "AspNetRoles",
         columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },

         values: new object[] { Guid.NewGuid().ToString(), "Broker", "Broker".ToUpper(), Guid.NewGuid().ToString() }
         );
          migrationBuilder.InsertData(
         table: "AspNetRoles",
         columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },

         values: new object[] { Guid.NewGuid().ToString(), "BrokerManager", "BrokerManager".ToUpper(), Guid.NewGuid().ToString() }
         );
           
          migrationBuilder.InsertData(
           table: "AspNetRoles",
           columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },

           values: new object[] { Guid.NewGuid().ToString(), "Tenant", "Tenant".ToUpper(), Guid.NewGuid().ToString() }
           );
            migrationBuilder.InsertData(
           table: "AspNetRoles",
           columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },

           values: new object[] { Guid.NewGuid().ToString(), "TemporaryTenant", "TemporaryTenant".ToUpper(), Guid.NewGuid().ToString() }
           );
            migrationBuilder.InsertData(
          table: "AspNetRoles",
          columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },

          values: new object[] { Guid.NewGuid().ToString(), "Seeker", "Seeker".ToUpper(), Guid.NewGuid().ToString() }
          );
            migrationBuilder.InsertData(
         table: "AspNetRoles",
         columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },

         values: new object[] { Guid.NewGuid().ToString(), " FutureTenant", " FutureTenant".ToUpper(), Guid.NewGuid().ToString() }
         );


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");

        }
    }
}
