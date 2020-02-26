using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieRental_DataAccess.Context;
using MovieRental_DataAccess.Repository;
using MovieRental_Infrastructure;
using MovieRental_Models;

namespace MovieRental_Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository, IPagination<Movie>
    {
        public MovieRepository(MovieRentalContext context) : base(context)
        {
                
        }

        public void AddLike(Movie entity, int quantity)
        {
            entity.Likes = (entity.Likes ?? 0) + quantity;
            Update(entity);
        }

        public bool CheckQuantityAvailable(int id, int quantity)
        {
            return (_context.Movies.Where(
               p => p.Id == id && (bool)p.Available && p.Stock >= quantity
               ).Count() > 0);
        }

        public IEnumerable<Movie> FindWithCreatedBy(Func<Movie, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Movie> GetAllAvailable(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _context.Movies.Where(p => (bool)p.Available && p.Stock > 0).AsQueryable();
            else
                return _context.Movies.Where(p => (bool)p.Available && p.Stock > 0 && p.Title.Contains(search)).AsQueryable();
        }

        public IEnumerable<Movie> GetByIdWithMovieLog(int id)
        {
            return _context.Movies.Where(p => p.Id == id).Include(p => p.MovieLogs).ToList();
        }

        public IQueryable<Movie> GetPaginated(string filter, int initialPage, int pageSize, string order, out int totalRecords, out int recordsFiltered, bool available)
        {
            var data = _context.Movies.AsQueryable();
            totalRecords = data.Count();

            data = data.Where(x => x.Available == available);

            if (!string.IsNullOrEmpty(filter))
            {
                data = data.Where(x => x.Title.ToUpper().Contains(filter.ToUpper()));
            }

            recordsFiltered = data.Count();

            if (order.ToUpper().Equals("ASC"))
                data = data.OrderBy(x => x.Likes)
                        .ThenBy(x => x.Title)
                        .Skip((initialPage * pageSize))
                        .Take(pageSize);
            else
                data = data.OrderByDescending(x => x.Likes)
                        .ThenBy(x => x.Title)
                        .Skip((initialPage * pageSize))
                        .Take(pageSize);

            return data;
        }
    }
}
