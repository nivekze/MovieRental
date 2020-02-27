using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace MovieRental_Models.DTO
{
    public class RentalDTO
    {
        public int? ClientId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int MovieQuantity { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }
    }
}
