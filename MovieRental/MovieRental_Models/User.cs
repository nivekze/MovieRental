using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Newtonsoft.Json;

namespace MovieRental_Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordSalt { get; set; }

        public virtual Role Role { get; set; }

        public bool Active { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

      
    }
}
