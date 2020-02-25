using System;
using System.Collections.Generic;
using System.Text;

using MovieRental_Models;

namespace MovieRental_Infrastructure
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllWithRol();

        User GetByIdWithRol(int id);

        User Authenticate(string username, string password);
    }
}
