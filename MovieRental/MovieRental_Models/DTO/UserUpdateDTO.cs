using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieRental_Models.DTO
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public int? RoleId { get; set; }
        public bool? Active { get; set; }
    }
}
