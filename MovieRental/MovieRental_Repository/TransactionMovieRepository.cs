using System;
using System.Collections.Generic;
using System.Text;

using MovieRental_DataAccess.Context;
using MovieRental_DataAccess.Repository;
using MovieRental_Infrastructure;
using MovieRental_Models;

namespace MovieRental_Repository
{
    public class TransactionMovieRepository : Repository<TransactionMovie>, ITransactionMovieRepository
    {
        public TransactionMovieRepository(MovieRentalContext context) : base(context)
        {

        }
    }
}
