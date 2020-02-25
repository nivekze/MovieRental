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
        public IActionResult GetProducts([FromQuery]int? draw, [FromQuery] int? start, [FromQuery] int? length, [FromQuery] string search, [FromQuery] int? page, [FromQuery] string order = "desc")
        {

            search = search ?? Request.Query["search[value]"].ToString();
            order = Request.Query["order[0][dir]"].ToString() ?? order;
            length = length ?? 10;
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue ? start / length : 0;
            start = page.HasValue ? page - 1 : start;

            try
            {
                var list = _movieRepository.GetPaginated(search, start.Value, length.Value, order, out totalRecords, out recordsFiltered);

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
        public ActionResult<Movie> GetProduct(int id)
        {
            var product = _movieRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}