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
                values: new object[] { "Rental" }
            );

            migrationBuilder.InsertData(
               table: "TransactionTypes",
               columns: new[] { "Name" },
               values: new object[] { "Purchase" }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"TRUNCATE TABLE TransactionTypes");
        }
    }
}
