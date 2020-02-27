using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MovieRental_Infrastructure;
using MovieRental_Models;

namespace MovieRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieLogRepository _movieLogRepository;
        public MoviesController(
            IMovieRepository movieRepository,
            IMovieLogRepository movieLogRepository)
        {
            _movieLogRepository = movieLogRepository;
            _movieRepository = movieRepository;
        }

        // GET: api/Movies
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMovies([FromQuery]int? draw, [FromQuery] int? start, [FromQuery] int? length, [FromQuery] string search, [FromQuery] int? page, [FromQuery] string order = "desc", [FromQuery] bool available = true)
        {

            search = search ?? Request.Query["search[value]"].ToString();
            order = Request.Query["order[0][dir]"].ToString() ?? order;
            length = length ?? 10;
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue ? start / length : 0;
            start = page.HasValue ? page - 1 : start;

            if (User.Identity.IsAuthenticated)
            {
                // Default behavior for normal user
                available = (User.Identity.IsAdmin())?available:true;
            }
            else {
                // Default behavior for anonymous user
                available = true;
            }

            
            try
            {
                var list = _movieRepository.GetPaginated(search, start.Value, length.Value, order, out totalRecords, out recordsFiltered, available);

                return new JsonResult(
                    new { draw, start, length, totalRecords, recordsFiltered, data = list }
                    );
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Movie> GetMovie(int id)
        {
            var movie = _movieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // GET: api/Movies/5
        [HttpGet("{id}/history")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Movie> GetMovieHistory(int id)
        {
            var movie = _movieRepository.GetByIdWithMovieLog(id);

            return movie;
        }

        // POST: api/Movies
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult<Movie> PostMovie(Movie movie)
        {
            movie.Available = movie.Available ?? true;
            movie.CreatedAt = DateTime.Now;
            movie.CreatedBy = int.Parse(User.Identity.Name);

            _movieRepository.Create(movie);

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            try
            {
                var updMovie = _movieRepository.GetById(id);
                updMovie.Title = movie.Title ?? updMovie.Title;
                updMovie.Description = movie.Description ?? updMovie.Description;
                //Save log if rental price is updated
                if (updMovie.RentalPrice.HasValue && movie.RentalPrice != null &&
                    !updMovie.RentalPrice.Equals(movie.RentalPrice))
                {
                    _movieLogRepository.Create(
                        new MovieLog
                        {
                            MovieId = updMovie.Id,
                            Column = "RentalPrice",
                            OldValue = updMovie.RentalPrice.Value.ToString(),
                            NewValue = movie.RentalPrice.Value.ToString(),
                            CreatedAt = DateTime.Now,
                            CreatedBy = int.Parse(User.Identity.Name) //Logged user
                        }
                    );
                }
                //Save log if sales price is updated
                if (updMovie.SalesPrice.HasValue && movie.SalesPrice != null &&
                    !updMovie.SalesPrice.Equals(movie.SalesPrice))
                {
                    _movieLogRepository.Create(
                        new MovieLog
                        {
                            MovieId = updMovie.Id,
                            Column = "SalesPrice",
                            OldValue = updMovie.SalesPrice.Value.ToString(),
                            NewValue = movie.SalesPrice.Value.ToString(),
                            CreatedAt = DateTime.Now,
                            CreatedBy = int.Parse(User.Identity.Name) //Logged user
                        }
                    );
                }
                //Save log if sales price is updated
                if (!string.IsNullOrWhiteSpace(updMovie.Title) && !string.IsNullOrWhiteSpace(movie.Title) &&
                    !updMovie.Title.Equals(movie.Title))
                {
                    _movieLogRepository.Create(
                        new MovieLog
                        {
                            MovieId = updMovie.Id,
                            Column = "Title",
                            OldValue = updMovie.Title,
                            NewValue = movie.Title,
                            CreatedAt = DateTime.Now,
                            CreatedBy = int.Parse(User.Identity.Name) //Logged user
                        }
                    );
                }

                updMovie.RentalPrice = movie.RentalPrice ?? updMovie.RentalPrice;
                updMovie.Stock = movie.Stock ?? updMovie.Stock;
                updMovie.Likes = movie.Likes ?? updMovie.Likes;
                updMovie.Available = movie.Available ?? updMovie.Available;
                updMovie.CreatedAt = movie.CreatedAt;
                updMovie.UpdatedAt = DateTime.Now;
                updMovie.UpdatedBy = int.Parse(User.Identity.Name);//Logged user

                _movieRepository.Update(updMovie);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public ActionResult<Movie> Delete(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            //If product has log 
            if (_movieLogRepository.Count(ml => ml.MovieId == movie.Id) > 0)
            {
                var movieLogs = _movieLogRepository.Find(ml => ml.MovieId == movie.Id).ToList();
                foreach (var item in movieLogs)
                {
                    _movieLogRepository.Delete(item);
                }
            }

            _movieRepository.Delete(movie);
            return movie;
        }

        // PUT: api/Movies/5/like
        [HttpPut("{id}/like")]
        public IActionResult LikeMovie(int id)
        {
            var updMovie = _movieRepository.GetById(id);
            if (updMovie == null)
            {
                return NotFound();
            }

            try
            {
                updMovie.UpdatedAt = DateTime.Now;
                updMovie.UpdatedBy = int.Parse(User.Identity.Name);
                _movieRepository.AddLike(updMovie, 1);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }
    }
}