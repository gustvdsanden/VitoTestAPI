using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using VitoTestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace VitoTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerrascopeController : ControllerBase
    {
        private static ApiContext _context;


        public TerrascopeController(ApiContext context)
        {
            _context = context;

        }
        // api/Terrascope/{boxid}/{scale}
        [HttpPost("{boxid}/{scale}")]
        public async Task<string> getGoodWeatherURL(int boxid,int scale)
        {
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID== boxid).OrderBy(b =>b.TimeStamp).LastAsync();
            string[] coords = measurement.Value.Split(";");
            double lon = double.Parse(coords[0]);
            double lat = double.Parse(coords[1]);
            List<string> result = await Helpers.Hexhelper.bboxcalc(lon, lat, (double)scale);
            return result[0];
        }
        [HttpGet("test/{boxid}")]
        public async Task<string> getGoodWeatherDate(int boxid)
        {
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == boxid).OrderBy(b => b.TimeStamp).LastAsync();
            string[] coords = measurement.Value.Split(";");
            double scale = 2000;
            double lon = double.Parse(coords[0]);
            double lat = double.Parse(coords[1]);
            List<string> result = await Helpers.Hexhelper.bboxcalc(lon, lat, (double)scale);
            return result[1];
        }
        // api/Terrascope/{boxid}
        [HttpPost("{boxid}")]
        public async Task<string> getDateFromDB(int boxid)
        {
            
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == boxid).OrderBy(b => b.TimeStamp).LastOrDefaultAsync();
            string date = "";
            if(measurement != null && measurement.Value.Length>0)
            {
                string[] coords = measurement.Value.Split(";");
                if (coords.Length==3)
                {
                    date = coords[2];
                }
                else
                {
                    date = await getGoodWeatherDate(boxid);
                    string[] fullCoords = new string[3];
                    fullCoords[0] = coords[0];
                    fullCoords[1] = coords[1];
                    fullCoords[2] = date;
                    measurement.Value = string.Join(";", fullCoords);
                    _context.Entry(measurement).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            } else
            {
                date = "No Location Data available";
            }

            return date;
        }
    }
}
