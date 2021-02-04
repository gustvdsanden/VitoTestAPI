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
            Box box = await _context.Boxes.FirstOrDefaultAsync(b=>b.BoxID  == boxid);
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID==5).OrderBy(b =>b.TimeStamp).LastAsync();
            string[] coords = measurement.Value.Split(";");
            double lon = double.Parse(coords[0]);
            double lat = double.Parse(coords[1]);
            List<string> result = await Helpers.Hexhelper.bboxcalc(lon, lat, (double)scale);
            return result[0];
        }
        // api/Terrascope/{boxid}
        [HttpPost("{boxid}")]
        public static async Task<string> getGoodWeatherDateLongLat(int boxid)
        {
            Box box = await _context.Boxes.FirstOrDefaultAsync(b => b.BoxID == boxid);
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == 5).OrderBy(b => b.TimeStamp).LastAsync();
            string[] coords = measurement.Value.Split(";");
            double scale = 2000;
            double lon = double.Parse(coords[0]);
            double lat = double.Parse(coords[1]);
            List<string> result = await Helpers.Hexhelper.bboxcalc(lon, lat, (double)scale);
            return result[1];
        }
    }
}
