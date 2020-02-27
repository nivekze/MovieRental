using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental_Models.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public int TokenExpiresIn { get; set; }
    }
}
