using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieRental_Models.DTO
{
    public class PurchaseDTO
    {
        public int? ClientId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int MovieQuantity { get; set; }
    }
}
