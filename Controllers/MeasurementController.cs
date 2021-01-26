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
    public class MeasurementController : ControllerBase
    {
        private ApiContext _context;

        public MeasurementController(ApiContext context)
        {
            _context = context;

        }
        //GET: api/Measurement

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
        {

            return await _context.Measurements.ToListAsync();
        }


        //GET: api/Measurement/{MeasurementID}

        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetMeasurementByID(int id)
        {
            return await _context.Measurements.FindAsync(id);
        }


        //GET: api/Measurement/SensorBox/{sensorid}/{boxid}
        [HttpGet("Sensor/{sensorid}/{boxid}")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementBySensorID(int sensorid, int boxid)
        {
            return await _context.Measurements.Where(m=>m.SensorID == sensorid && m.BoxID == boxid).Include(sb => sb.SensorBox).ThenInclude(s=>s.Sensor).ThenInclude(sb=>sb.SensorType).ToListAsync();
        }


        //POST: api/Measurement
        [HttpPost]
        public async Task<ActionResult<Measurement>> PostMeasurement(Measurement measurement)
        {
            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();

            return Ok(measurement);
        }

        // PUT: api/Measurement/{MeasurementID}
        [HttpPut("{id}")]

        public async Task<ActionResult<Measurement>> PutMeasurement(int id, Measurement measurement)
        {
            if (id != measurement.MeasurementID)
            {
                return BadRequest();
            }

            _context.Entry(measurement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasurementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return measurement;
        }

        //DELETE: api/Measurement/{MeasurementID}
        [HttpDelete("{id}")]

        public async Task<ActionResult<Measurement>> DeleteMeasurement(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }

            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();

            return measurement;
        }

        private bool MeasurementExists(int id)
        {
            return _context.Measurements.Any(e => e.MeasurementID == id);
        }
    }
}
