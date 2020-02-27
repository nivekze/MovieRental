using System;
using System.Collections.Generic;
using System.Text;

using MovieRental_Models;

namespace MovieRental_Infrastructure
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Transaction GetByIdWithMovies(int id);
    }
}
