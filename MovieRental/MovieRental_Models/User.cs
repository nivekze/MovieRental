using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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

        [JsonIgnore]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool Active { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

      
    }

    public static class UserExtensios {
        public static bool IsAdmin(this IIdentity identity)
        {
            var roles = ((ClaimsIdentity)identity).Claims
               .Where(c => c.Type == ClaimTypes.Role)
               .Select(c => c.Value);

            return roles.Contains("Admin");
        }
    }
}
