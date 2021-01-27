using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoTestAPI.Services;
using VitoTestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;


namespace VitoTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private ApiContext _context;
 
        public UserController(IUserService userService,ApiContext context)
        {
            _userService = userService;
            _context = context;

        }

        // POST: api/User/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }
        //GET: api/User

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //GET: api/User/{userID}

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsersByID(int id)
        {
            return await _context.Users.FindAsync(id);
        }
       

        //GET: api/User/admins

        [HttpGet("admins")]
        public async Task<ActionResult<IEnumerable<User>>> getAdmins()
        {
            var users = await _context.Users.ToListAsync();
            var admins = new ObservableCollection<User>();
            foreach (var user in users)
            {
                if (user.UserTypeID == 1)
                {
                    admins.Add(user);
                }
            }
            return admins;
        }

        //POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {



            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // PUT: api/User/{userID}
        [HttpPut("{id}")]
  
        public async Task<ActionResult<User>> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return user;
        }

        //DELETE: api/User/{userID}
        [HttpDelete("{id}")]
 
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
