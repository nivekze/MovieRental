using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

using MovieRental_Models;

namespace MovieRental_DataAccess.Context
{
    public class MovieRentalContext : DbContext
    {
        #region Tables
        
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieLog> MovieLogs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionMovie> TransactionMovies { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        #endregion

        public MovieRentalContext(DbContextOptions<MovieRentalContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
