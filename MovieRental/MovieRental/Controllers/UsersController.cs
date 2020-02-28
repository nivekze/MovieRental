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
using MovieRental_Notification;

namespace MovieRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;

        public UsersController(
            IUserRepository userRepository,
            INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDTO userParam)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
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
            try
            {
                var user = _userRepository.GetByIdWithRol(id);
                user.Password = null;
                user.PasswordSalt = null;

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, UserUpdateDTO user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest();
                }
                var updUser = _userRepository.GetById(id);

                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    updUser.PasswordSalt = PasswordHasher.GetSalt();
                    updUser.Password = PasswordHasher.GetHash(user.Password + updUser.PasswordSalt);
                }

                updUser.Active = user.Active ?? updUser.Active;
                updUser.RoleId = user.RoleId ?? updUser.RoleId;

                _userRepository.Update(updUser);

                return Ok(new { Message = "User updated!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult<User> PostUser(UserDTO user)
        {
            try
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
                var baseUrl = $"{Request.Scheme}://{Request.Host}/api/users/{newUser.Username}/confirm-account";

                _notificationRepository.SendEmailConfirmingAccount(newUser.Username, "Movie Rental - Confirming email for new account", baseUrl);
                return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUser(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }

                _userRepository.Delete(user);

                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // PUT: api/Users/example@test.dev/confirm-account
        [HttpGet("{username}/confirm-account")]
        [AllowAnonymous]
        public IActionResult GetConfirmAccount(string username)
        {
            try
            {
                try
                {
                    var user = _userRepository.Find(u => u.Username.Equals(username)).FirstOrDefault();
                    user.EmailConfirmed = true;
                    _userRepository.Update(user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Ok(new { 
                    Message = "Email confirmed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // PUT: api/Users/example@test.dev/recovery-password
        [HttpGet("{username}/recovery-password")]
        [AllowAnonymous]
        public IActionResult GetRecoveryPassword(string username)
        {
            try
            {
                try
                {
                    var user = _userRepository.Find(u => u.Username.Equals(username)).FirstOrDefault();

                    if (user == null || string.IsNullOrWhiteSpace(user.Username))
                    {
                        return NotFound();
                    }
                    var newPassword = RandomGenerator.RandomPassword();
                    user.PasswordSalt = PasswordHasher.GetSalt();
                    user.Password = PasswordHasher.GetHash(newPassword + user.PasswordSalt);
                    _userRepository.Update(user);
                    _notificationRepository.SendEmailRecoveryPassword(user.Username, "Movie Rental - Confirming email for new account", newPassword);

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Ok(new
                {
                    Message = "We sent the new password to your email"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}