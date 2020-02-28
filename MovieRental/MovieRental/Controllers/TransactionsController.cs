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
using MovieRental_Models.DTO;

namespace MovieRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionsController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionMovieRepository _transactionMovieRepository;
        public TransactionsController(
            IMovieRepository movieRepository,
            ITransactionRepository transactionRepository,
            ITransactionMovieRepository transactionMovieRepository)
        {
            _movieRepository = movieRepository;
            _transactionRepository = transactionRepository;
            _transactionMovieRepository = transactionMovieRepository;
        }

        // POST: api/Transactions/purchase

        [HttpPost("purchase")]
        public ActionResult<Transaction> PostPurchase(List<PurchaseDTO> purchaseDetail)
        {
            try
            {
                if (!ModelState.IsValid && purchaseDetail != null && purchaseDetail.Count > 0)
                    return BadRequest(new { Message = "Required data missing" });

                //Check available quantity
                foreach (var item in purchaseDetail)
                {
                    //Validation
                    if (item.MovieQuantity <= 0)
                        return BadRequest(new
                        {
                            Message = string.Concat("MovieId <", item.MovieId, "> quantity must be greater  than 0")
                        });

                    if (!_movieRepository.CheckQuantityAvailable(item.MovieId, item.MovieQuantity))
                        return BadRequest(new
                        {
                            Message = string.Concat("MovieId <", item.MovieId, "> unavailable or insufficient  stock")
                        });
                }
                var userId = int.Parse(User.Identity.Name);
                var clientId = purchaseDetail.First().ClientId ?? userId;

                var newTransaction = new Transaction();
                newTransaction.ClientId = clientId;
                newTransaction.TransactionTypeId = 2;//Purchase
                newTransaction.MoviesQuantity = purchaseDetail.Count();
                newTransaction.Total = 0; //Zero by default
                newTransaction.CreatedAt = DateTime.Now;
                newTransaction.CreatedBy = userId;

                _transactionRepository.Create(newTransaction);


                foreach (var item in purchaseDetail)
                {
                    var movie = _movieRepository.GetById(item.MovieId);

                    //Create purchase detail
                    var newTransactionMovie = new TransactionMovie();
                    newTransactionMovie.TransactionId = newTransaction.Id;
                    newTransactionMovie.MovieId = movie.Id;
                    newTransactionMovie.ProductQuantity = item.MovieQuantity;
                    newTransactionMovie.Price = (decimal)movie.SalesPrice;
                    newTransactionMovie.SubTotal = decimal.Round((newTransactionMovie.ProductQuantity * movie.SalesPrice.Value), 2);
                    newTransactionMovie.CreatedBy = userId;
                    newTransactionMovie.CreatedAt = DateTime.Now;

                    _transactionMovieRepository.Create(newTransactionMovie);

                    //Update total purchase
                    newTransaction.Total += newTransactionMovie.SubTotal;

                    //Decrement product stock
                    movie.Stock -= newTransactionMovie.ProductQuantity;
                    movie.UpdatedAt = DateTime.Now;
                    movie.UpdatedBy = userId;

                    _movieRepository.Update(movie);

                }
                //Update total purchase
                newTransaction.UpdatedAt = DateTime.Now;
                newTransaction.UpdatedBy = userId;
                _transactionRepository.Update(newTransaction);

                return CreatedAtAction("GetTransaction", new { id = newTransaction.Id }, newTransaction);
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public ActionResult<Transaction> GetTransaction(int id)
        {
            try
            {
                var transaction = _transactionRepository.GetByIdWithMovies(id);

                if (transaction == null)
                {
                    return NotFound();
                }

                return transaction;
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // POST: api/Transactions/purchase

        [HttpPost("rental")]
        public ActionResult<Transaction> PostRental(List<RentalDTO> rentalDetail)
        {
            try
            {
                if (!ModelState.IsValid && rentalDetail != null && rentalDetail.Count > 0)
                    return BadRequest(new { Message = "Required data missing" });
                var transactionDate = DateTime.Now;
                //Check available quantity
                foreach (var item in rentalDetail)
                {
                    //Validation
                    if (item.MovieQuantity <= 0)
                        return BadRequest(new
                        {
                            Message = string.Concat("MovieId <", item.MovieId, "> quantity must be greater  than 0")
                        });

                    if (!_movieRepository.CheckQuantityAvailable(item.MovieId, item.MovieQuantity))
                        return BadRequest(new
                        {
                            Message = string.Concat("MovieId <", item.MovieId, "> unavailable or insufficient  stock")
                        });

                    if (item.ReturnDate <= transactionDate)
                        return BadRequest(new
                        {
                            Message = string.Concat("MovieId <", item.MovieId, "> return date is invalid")
                        });
                }
                var userId = int.Parse(User.Identity.Name);
                var clientId = rentalDetail.First().ClientId ?? userId;

                var newTransaction = new Transaction();
                newTransaction.ClientId = clientId;
                newTransaction.TransactionTypeId = 1;//Rental
                newTransaction.MoviesQuantity = rentalDetail.Count();
                newTransaction.Total = 0; //Zero by default
                newTransaction.ReturnScheduleDate = rentalDetail.First().ReturnDate;
                newTransaction.CreatedAt = DateTime.Now;
                newTransaction.CreatedBy = userId;

                _transactionRepository.Create(newTransaction);


                foreach (var item in rentalDetail)
                {
                    var movie = _movieRepository.GetById(item.MovieId);
                    var days = (int)(item.ReturnDate - transactionDate).TotalDays;
                    //Create purchase detail
                    var newTransactionMovie = new TransactionMovie();
                    newTransactionMovie.TransactionId = newTransaction.Id;
                    newTransactionMovie.MovieId = movie.Id;
                    newTransactionMovie.ProductQuantity = item.MovieQuantity;
                    newTransactionMovie.Price = (decimal)movie.RentalPrice * days;
                    newTransactionMovie.SubTotal = decimal.Round((newTransactionMovie.ProductQuantity * movie.RentalPrice.Value), 2);
                    newTransactionMovie.CreatedBy = userId;
                    newTransactionMovie.CreatedAt = DateTime.Now;

                    _transactionMovieRepository.Create(newTransactionMovie);

                    //Update total purchase
                    newTransaction.Total += newTransactionMovie.SubTotal;

                    //Decrement product stock
                    movie.Stock -= newTransactionMovie.ProductQuantity;
                    movie.UpdatedAt = DateTime.Now;
                    movie.UpdatedBy = userId;

                    _movieRepository.Update(movie);

                }
                //Update total purchase
                newTransaction.UpdatedAt = DateTime.Now;
                newTransaction.UpdatedBy = userId;
                _transactionRepository.Update(newTransaction);

                return CreatedAtAction("GetTransaction", new { id = newTransaction.Id }, newTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("rental/{id}/return")]
        public ActionResult<Transaction> PutRental(int id)
        {
            try
            {
                var transaction = _transactionRepository.GetByIdWithMovies(id);

                if (transaction == null || transaction.TransactionTypeId != 1)
                {
                    return NotFound();
                }

                if(transaction.ReturnedDate.HasValue)
                    return BadRequest(new
                    {
                        Message = string.Concat("TransactionId <", transaction.Id, "> already returned")
                    });

                var userId = int.Parse(User.Identity.Name);
                var transactionDate = DateTime.Now;
                var penaltyDays = (int)(transaction.ReturnScheduleDate.Value - transactionDate).TotalDays;
                penaltyDays = (penaltyDays < 0) ? penaltyDays *-1:0;

                foreach (var item in transaction.Movies)
                {
                    var movie = _movieRepository.GetById(item.MovieId);

                   //Increment product stock
                    movie.Stock += item.ProductQuantity;
                    movie.UpdatedAt = DateTime.Now;
                    movie.UpdatedBy = userId;

                    _movieRepository.Update(movie);

                    transaction.Penalty += movie.PenaltyPerDay * penaltyDays;

                }
                //Update total purchase
                transaction.Total += transaction.Penalty ?? 0;
                transaction.ReturnedDate = transactionDate;
                transaction.UpdatedAt = DateTime.Now;
                transaction.UpdatedBy = userId;
                _transactionRepository.Update(transaction);

                return AcceptedAtAction("GetTransaction", new { id = transaction.Id }, transaction);

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}