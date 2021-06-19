using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRental_AWSSecretManager
{
    public class DbSecrectConnetion
    {
        public string username { get; set; }
        public string password { get; set; }
        public string engine { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public string dbInstanceIdentifier { get; set; }
    }
}
