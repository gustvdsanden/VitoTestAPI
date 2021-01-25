using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VitoTestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VitoTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private ApiContext _context;

        public UserTypeController(ApiContext context)
        {
            _context = context;

        }
        //GET: api/UserType

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserType>>> GetUserTypes()
        {

            return await _context.UserTypes.ToListAsync();
        }


        //GET: api/UserType/{UserTypeID}

        [HttpGet("{id}")]
        public async Task<ActionResult<UserType>> GetUserTypeByID(int id)
        {
            return await _context.UserTypes.FindAsync(id);
        }


        //POST: api/UserType
        [HttpPost]
        public async Task<ActionResult<UserType>> PostUserType(UserType userType)
        {
            _context.UserTypes.Add(userType);
            await _context.SaveChangesAsync();

            return Ok(userType);
        }

        // PUT: api/UserType/{UserTypeID}
        [HttpPut("{id}")]

        public async Task<ActionResult<UserType>> PutUserType(int id, UserType userType)
        {
            if (id != userType.UserTypeID)
            {
                return BadRequest();
            }

            _context.Entry(userType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return userType;
        }

        //DELETE: api/UserType/{UserTypeID}
        [HttpDelete("{id}")]

        public async Task<ActionResult<UserType>> DeleteUserType(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }

            _context.UserTypes.Remove(userType);
            await _context.SaveChangesAsync();

            return userType;
        }

        private bool UserTypeExists(int id)
        {
            return _context.UserTypes.Any(e => e.UserTypeID == id);
        }
    }
}
