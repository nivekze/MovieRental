using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieRental_Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public int Stock { get; set; }
        
        public int? Likes { get; set; }
        
        public decimal RentalPrice { get; set; }
        
        public decimal SalesPrice { get; set; }
        
        public bool Available { get; set; }

        public string[] Images { get; set; }

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
