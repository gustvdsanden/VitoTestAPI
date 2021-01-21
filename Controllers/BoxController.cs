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
    public class BoxController : ControllerBase
    {
        private ApiContext _context;

        public BoxController(ApiContext context)
        {
            _context = context;

        }
        //GET: api/Box
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Box>>> GetBoxes()
        {

            return await _context.Boxes.ToListAsync();
        }

        //GET: api/Box/{BoxID}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Box>> GetBoxByID(int id)
        {
            return await _context.Boxes.FindAsync(id);
        }


        //POST: api/Box
        [HttpPost]
        public async Task<ActionResult<Box>> PostBox(Box box)
        {
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();

            return Ok(box);
        }

        // PUT: api/Box/{BoxID}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Box>> PutBox(int id, Box box)
        {
            if (id != box.BoxID)
            {
                return BadRequest();
            }

            _context.Entry(box).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoxExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return box;
        }

        //DELETE: api/Box/{BoxID}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Box>> DeleteBox(int id)
        {
            var box = await _context.Boxes.FindAsync(id);
            if (box == null)
            {
                return NotFound();
            }

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();

            return box;
        }

        private bool BoxExists(int id)
        {
            return _context.Boxes.Any(e => e.BoxID == id);
        }
    }
}
