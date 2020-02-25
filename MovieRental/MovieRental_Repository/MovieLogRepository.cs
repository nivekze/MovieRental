using System;
using System.Collections.Generic;
using System.Text;

using MovieRental_DataAccess.Context;
using MovieRental_DataAccess.Repository;
using MovieRental_Infrastructure;
using MovieRental_Models;

namespace MovieRental_Repository
{
    public class MovieLogRepository : Repository<MovieLog>, IMovieLogRepository
    {
        public MovieLogRepository(MovieRentalContext context) : base(context)
        {

        }
    }
}
