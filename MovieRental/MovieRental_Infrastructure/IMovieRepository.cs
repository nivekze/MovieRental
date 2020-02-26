using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MovieRental_Models;

namespace MovieRental_Infrastructure
{
    public interface IMovieRepository : IRepository<Movie>, IPagination<Movie>
    {
        IEnumerable<Movie> FindWithCreatedBy(Func<Movie, bool> predicate);
        IEnumerable<Movie> GetByIdWithMovieLog(int id);

        void AddLike(Movie entity, int quantity);

        IQueryable<Movie> GetAllAvailable(string search);

        bool CheckQuantityAvailable(int id, int quantity);

    }
}
