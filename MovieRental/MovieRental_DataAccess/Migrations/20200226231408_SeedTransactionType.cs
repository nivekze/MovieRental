using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieRental_DataAccess.Migrations
{
    public partial class SeedTransactionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Name"},
                values: new object[] { "Rental", true, DateTime.Now }
            );

            migrationBuilder.InsertData(
               table: "TransactionTypes",
               columns: new[] { "Name" },
               values: new object[] { "Purchase", true, DateTime.Now }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"TRUNCATE TABLE TransactionTypes");
        }
    }
}
