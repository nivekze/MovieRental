using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MovieRental_DataAccess.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "Roles",
             columns: new[] { "Name", "Active", "CreatedAt" },
             values: new object[] { "Admin", true, DateTime.Now }
         );

            migrationBuilder.InsertData(
               table: "Roles",
               columns: new[] { "Name", "Active", "CreatedAt" },
               values: new object[] { "Client", true, DateTime.Now }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"TRUNCATE TABLE Roles");
        }
    }
}
