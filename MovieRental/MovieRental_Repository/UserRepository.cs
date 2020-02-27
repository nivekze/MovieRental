using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using MovieRental_Models;
using MovieRental_Models.Helpers;
using MovieRental_Infrastructure;
using MovieRental_DataAccess.Context;
using MovieRental_DataAccess.Repository;

namespace MovieRental_Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppSettings _appSettings;

        public UserRepository(MovieRentalContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            _appSettings = appSettings.Value;
        }


        public IEnumerable<User> GetAllWithRol()
        {
            return _context.Users.Include(u => u.Role);
        }

        public User GetByIdWithRol(int id)
        {
            return _context.Users.Where(u => u.Id == id)
                .Include(u => u.Role)
                .SingleOrDefault();
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.Include(r => r.Role)
                .SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddSeconds(_appSettings.TokenExpiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }
    }
}
