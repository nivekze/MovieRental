using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieRental_Infrastructure;
using MovieRental_Models;

namespace MovieRental.Controllers
{
    public class MovieRentalController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MovieRentalController(
              IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult View(int id)
        {
            var movie = _movieRepository.GetById(id);
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie = _movieRepository.GetById(id);
            return View(movie);
        }

        public IActionResult New()
        {
            var movie = new Movie();
            movie.SalesPrice = 0;
            movie.RentalPrice = 0;
            movie.PenaltyPerDay = 0;
            movie.Stock = 0;
            movie.Likes = 0;

            return View(movie);
        }

        [HttpPost]
        public IActionResult Update(Movie movie) {
            try
            {
                var dbMovie = _movieRepository.GetById(movie.Id);
                dbMovie.Title = movie.Title;
                dbMovie.Description = movie.Description;
                dbMovie.Stock = movie.Stock;
                dbMovie.SalesPrice = movie.SalesPrice;
                dbMovie.RentalPrice = movie.RentalPrice;
                dbMovie.Likes = movie.Likes;
                dbMovie.PenaltyPerDay = movie.PenaltyPerDay;
                dbMovie.UpdatedAt = DateTime.Now;
                _movieRepository.Update(dbMovie);
                return Json(new { 
                    success = true
                });
            }
            catch (Exception ex) {
                return Json(new { 
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult Save(Movie movie)
        {
            try
            {
                movie.Available = true;
                movie.CreatedBy = 1;
                movie.CreatedAt = DateTime.Now;
                _movieRepository.Create(movie);
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public IActionResult Delete(int id)
        {
            var movie = _movieRepository.GetById(id);
            _movieRepository.Delete(movie);

            return RedirectToAction("Index");
        }
    }
}