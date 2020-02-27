using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Microsoft.EntityFrameworkCore;

using MovieRental_Models;
using MovieRental_Infrastructure;
using MovieRental_DataAccess.Context;
using MovieRental_DataAccess.Repository;

namespace MovieRental_Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MovieRentalContext context) : base(context)
        {
                
        }
        public Transaction GetByIdWithMovies(int id)
        {
            return _context.Transactions.Where(p => p.Id == id)
              .Include(type => type.TransactionType)
              .Include(detail => detail.Movies)
                .ThenInclude(movie => movie.Movie)
              .First();
        }
    }
}
