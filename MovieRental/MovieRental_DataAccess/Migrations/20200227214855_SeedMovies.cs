using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MovieRental_DataAccess.Migrations
{
    public partial class SeedMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var moviesColumns = new[] { "Title", "Description", "Stock", "Likes", "RentalPrice", "SalesPrice", "PenaltyPerDay", "Available", "Images", "CreatedAt", "CreatedBy" };

            migrationBuilder.InsertData(
               table: "Movies",
               columns: moviesColumns,
               values: new object[] { 
                   "The Grudge","The Grudge Description",100,0,2.75,59.99,1.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
           );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "Underwater","Underwater Description",50,0,1.75,49.99,1.75,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
          );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "Like a Boss","Like a Boss Description",110,0,3.75,79.99,2.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg",
                    DateTime.Now, 1}
          );

            migrationBuilder.InsertData(
                 table: "Movies",
                 columns: moviesColumns,
                 values: new object[] {
                   "The Gentlemen","The Gentlemen Description",100,0,2.75,59.99,1.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
             );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "The Turning","The Turning Description",50,0,1.75,49.99,1.75,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
          );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "The Lodge","The Lodge Description",110,0,3.75,79.99,2.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg",
                    DateTime.Now, 1}
          );

            migrationBuilder.InsertData(
                 table: "Movies",
                 columns: moviesColumns,
                 values: new object[] {
                   "Downhill","Downhill Description",100,0,2.75,59.99,1.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
             );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "Brahms: The Boy II","Brahms: The Boy II Description",50,0,1.75,49.99,1.75,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
          );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "TBloodshot","Bloodshot Description",110,0,3.75,79.99,2.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg",
                    DateTime.Now, 1}
          );

            migrationBuilder.InsertData(
             table: "Movies",
             columns: moviesColumns,
             values: new object[] {
                   "Fantasy Island","Fantasy Island Description",50,0,1.75,49.99,1.75,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg|https://files.erasmusmagazine.nl/app/uploads/sites/8/2017/05/09202615/Right-Movie-Night-1.jpg",
                    DateTime.Now, 1}
         );

            migrationBuilder.InsertData(
              table: "Movies",
              columns: moviesColumns,
              values: new object[] {
                   "The Photograph","The Photograph* Description",110,0,3.75,79.99,2.5,true,
                   "https://media.wired.com/photos/5b7350e75fc74d47846ce4e4/master/w_2560%2Cc_limit/Popcorn-869302844.jpg",
                    DateTime.Now, 1}
          );


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"TRUNCATE TABLE Movies");
        }
    }
}
