using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitoTestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VitoTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private ApiContext _context;

        public SensorController(ApiContext context)
        {
            _context = context;

        }
        //GET: api/Sensor

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {

            return await _context.Sensors.ToListAsync();
        }
        //GET: api/Sensor/Box

        [HttpGet("Box")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensorsWithBoxes()
        {

            return await _context.Sensors.ToListAsync();
        }

        //GET: api/Sensor/{SensorID}

        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensorByID(int id)
        {
            return await _context.Sensors.FindAsync(id);
        }


        //POST: api/Sensor
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();

            return Ok(sensor);
        }

        // PUT: api/Sensor/{SensorID}
        [HttpPut("{id}")]

        public async Task<ActionResult<Sensor>> PutSensor(int id, Sensor sensor)
        {
            if (id != sensor.SensorID)
            {
                return BadRequest();
            }

            _context.Entry(sensor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return sensor;
        }

        //DELETE: api/Sensor/{BoxID}
        [HttpDelete("{id}")]

        public async Task<ActionResult<Sensor>> DeleteSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();

            return sensor;
        }

        private bool SensorExists(int id)
        {
            return _context.Sensors.Any(e => e.SensorID == id);
        }
    }
}
