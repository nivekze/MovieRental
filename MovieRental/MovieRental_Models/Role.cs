using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental_Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
