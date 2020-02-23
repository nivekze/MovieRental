using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRental_Models
{
    public class TransactionMovie
    {
        public int Id { get; set; }

        public int ProductQuantity { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal SubTotal { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public int TransactionId { get; set; }

        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; }
        
        [JsonIgnore]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual User CreatedByUser { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual User UpdatedByUser { get; set; }

    }
}
