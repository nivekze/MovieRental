using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRental_Models
{
    public class MovieLog
    {
        public int Id { get; set; }

        public string Column { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
        
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public virtual User CreatedByUser { get; set; }
    }
}
