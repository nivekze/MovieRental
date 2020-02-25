using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRental_Infrastructure;
using MovieRental_Models;
using MovieRental_Models.DTO;
using MovieRental_Repository.Helpers;

namespace MovieRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDTO userParam)
        {
            var userSalt = _userRepository.Find(u => u.Username == userParam.Username).SingleOrDefault();

            if (userSalt == null)
                return BadRequest(new { Message = "User not exits" });

            userParam.Password = PasswordHasher.GetHash(userParam.Password + userSalt.PasswordSalt);

            var user = _userRepository.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetAllWithRol();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userRepository.GetByIdWithRol(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }


            try
            {
                _userRepository.Update(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult<User> PostUser(UserDTO user)
        {
            if (_userRepository.Count(u => u.Username.Equals(user.Username)) > 0)
                return BadRequest(new { Message = "User alredy exists" });

            var newUser = new User();
            newUser.Username = user.Username;
            newUser.Password = user.Password;
            newUser.RoleId = user.RoleId;
            newUser.Active = user.Active ?? true;
            newUser.PasswordSalt = PasswordHasher.GetSalt();
            newUser.Password = PasswordHasher.GetHash(user.Password + newUser.PasswordSalt);
            newUser.CreatedAt = DateTime.Now;

            _userRepository.Create(newUser);

            return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.Delete(user);

            return user;
        }
    }
}