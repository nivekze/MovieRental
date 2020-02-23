using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRental_Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int TransactionTypeId { get; set; }

        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; }

        public int MoviesQuantity { get; set; }
        
        public decimal Total { get; set; }

        public DateTime? ReturnScheduleDate { get; set; }
        
        public DateTime? ReturnedDate { get; set; }

        public decimal? Penalty { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual User Client { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }
        
        [ForeignKey("CreatedBy")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual User CreatedByUser { get; set; }

        public DateTime? UpdatedAt { get; set; }
        
        public int? UpdatedBy { get; set; }
        
        [ForeignKey("UpdatedBy")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual User UpdatedByUser { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<TransactionMovie> Movies { get; set; }
    }
}
