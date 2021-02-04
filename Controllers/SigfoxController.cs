using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using VitoTestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VitoTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigFoxController : ControllerBase
    {
        private ApiContext _context;




        public SigFoxController(ApiContext context)
        {
            _context = context;



        }

        //POST: api/Sigfox/PostData/{callback}/{device}
        [HttpPost("PostData/{callback}/{device}")]
        public async Task<string> PostSigfoxData(string callback, string device)
        {
            //DateTime date1 = DateTime.UtcNow;
            //TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            //DateTime date2 = TimeZoneInfo.ConvertTime(date1, timeZoneInfo);
            Box box = _context.Boxes.FirstOrDefault(b => b.MacAddress == device);
            if (box == null)
            {
                return "Box not found";
            }
            List<string> sigfoxData = Helpers.Hexhelper.HexConv(callback);
            if (sigfoxData[0] == "1")
            {
                sigfoxData.Remove("1");
                sigfoxData.RemoveAll(p => p == "0");
                List<string> dummyData = sigfoxData;
                foreach (string sensor in dummyData)
                {
                    if (await _context.SensorBoxes.FindAsync(box.BoxID, int.Parse(sensor)) == null)
                    {
                        SensorBox sensorbox = new SensorBox();
                        sensorbox.BoxID = box.BoxID;
                        sensorbox.SensorID = int.Parse(sensor);
                        _context.SensorBoxes.Add(sensorbox);
                    }
                }

                box.ConfiguratieString = string.Join(",", sigfoxData);
                await _context.SaveChangesAsync();
                return "Configuratie aanvaard";
            } else if ((sigfoxData[0] == "2") || (sigfoxData[0] == "4"))
            {
                string configuratieString = box.ConfiguratieString;
                string[] sensors = configuratieString.Split(',');
                if (sigfoxData[0] == "4")
                {
                    box.Comment = "Geen Gps beschikbaar";
                    _context.Entry(box).State = EntityState.Modified;
                }
                sigfoxData.Remove(sigfoxData[0]);
                foreach (string sensor in sensors)
                {
                    if (sensor != "0")
                    {
                        if (sensor == "20")
                        {
                            sigfoxData[0] = (int.Parse(sigfoxData[0]) - 30).ToString();
                        }
                        Measurement measurement = new Measurement();
                        measurement.SensorID = int.Parse(sensor);
                        measurement.BoxID = box.BoxID;
                        measurement.TimeStamp = DateTime.Now;
                        measurement.Value = sigfoxData[0];
                        sigfoxData.Remove(sigfoxData[0]);
                        _context.Measurements.Add(measurement);

                    }
                }
                await _context.SaveChangesAsync();
                //meting
                return "Meting opgeslagen";
            }
            else if (sigfoxData[0] == "3")
            {
                //snapshot
                sigfoxData.Remove("3");

                Monitoring monitoring = new Monitoring();
                monitoring.TimeStamp = DateTime.Now;
                monitoring.BoxID = box.BoxID;
                //batterij
                if (int.Parse(sigfoxData[0]) >= 0 || int.Parse(sigfoxData[0]) <= 100) {
                    monitoring.BatteryPercentage = sigfoxData[0];
                }
                else if (sigfoxData[0] == "253")
                {
                    monitoring.BatteryPercentage = "No Read";
                }
                else if (sigfoxData[0] == "254")
                {
                    monitoring.BatteryPercentage = "Overvolted";
                }
                else if (sigfoxData[0] == "255")
                {
                    monitoring.BatteryPercentage = "Undervolted";
                }
                //SD card
                if (int.Parse(sigfoxData[1]) >= 0 || int.Parse(sigfoxData[1]) <= 100)
                {
                    monitoring.SdCapacity = sigfoxData[1];
                }
                else if (sigfoxData[1] == "253")
                {
                    monitoring.BatteryPercentage = "SD Empty/Corrupt";
                }
                else if (sigfoxData[1] == "254")
                {
                    monitoring.BatteryPercentage = "Wrong format (use: PAT)";
                }
                else if (sigfoxData[1] == "255")
                {
                    monitoring.BatteryPercentage = "No Read";
                }
                //satellite
                monitoring.AmountSatellite = sigfoxData[2];
                //temperatuur
                monitoring.Temperature = sigfoxData[3];
                //GPS
                Measurement measurement = new Measurement();
                if (await _context.SensorBoxes.FindAsync(box.BoxID, 17) == null)
                {
                    SensorBox sensorbox = new SensorBox();
                    sensorbox.BoxID = box.BoxID;
                    sensorbox.SensorID = 17;
                    _context.SensorBoxes.Add(sensorbox);
                    await _context.SaveChangesAsync();
                }
                measurement.BoxID = box.BoxID;
                measurement.SensorID = 17;
                measurement.TimeStamp = DateTime.Now;
                string datum = await getGoodWeatherDate(box.BoxID);
                measurement.Value = sigfoxData[4] + ";" + sigfoxData[5] + ";" + datum;
                _context.Measurements.Add(measurement);
                _context.Monitorings.Add(monitoring);
                await _context.SaveChangesAsync();
                return string.Join(',', sigfoxData);
            }
            return "Identifier not caught";
        }
        // api/Sigfox/{boxid}/{scale}
        [HttpPost("{boxid}/{scale}")]
        public async Task<string> getGoodWeatherURL(int boxid, int scale)
        {
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == boxid).OrderBy(b => b.TimeStamp).LastAsync();
            string[] coords = measurement.Value.Split(";");
            double lon = double.Parse(coords[0]);
            double lat = double.Parse(coords[1]);
            List<string> result = await Helpers.Hexhelper.bboxcalc(lon, lat, (double)scale);
            return result[0];
        }
        // api/Terrascope/{boxid}
        [HttpPost("{boxid}")]
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
        // api/Terrascope/DB/{boxid}
        [HttpPost("DB/{boxid}")]
        public async Task<string> getDateFromDB(int boxid)
        {

            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == boxid).OrderBy(b => b.TimeStamp).LastOrDefaultAsync();
            string date = "";
            if (measurement != null && measurement.Value.Length > 0)
            {
                string[] coords = measurement.Value.Split(";");
                if (coords.Length == 3)
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
            }
            else
            {
                date = "No Location Data available";
            }

            return date;
        }
    }
}


