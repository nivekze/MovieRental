using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MovieRental_DataAccess.Migrations
{
    public partial class SeedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var userSalt = GetSalt();
            var userPassword = GetHash("k3vin19*" + userSalt);
            migrationBuilder.InsertData(
               table: "Users",
               columns: new[] { "Username", "Password", "PasswordSalt", "RoleId", "Active", "CreatedAt" },
               values: new object[] { "admin", userPassword, userSalt, 1, true, DateTime.Now }
           );

            userSalt = GetSalt();
            userPassword = GetHash("1234" + userSalt);
            migrationBuilder.InsertData(
               table: "Users",
               columns: new[] { "Username", "Password", "PasswordSalt", "RoleId", "Active", "CreatedAt" },
               values: new object[] { "client", userPassword, userSalt, 2, true, DateTime.Now }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"TRUNCATE TABLE Users");
        }

        private static string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
