using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRental_Infrastructure;
using MovieRental_Models.DTO;
using MovieRental_Repository.Helpers;

namespace MovieRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}