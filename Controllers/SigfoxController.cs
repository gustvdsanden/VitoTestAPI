using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using VitoTestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using BAMCIS.GeoJSON;
using Newtonsoft.Json;
using System.Text;


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
        public async Task<List<string>> PostSigfoxData(string callback, string device)
        {
            List<string> returnList = new List<string>();
            DateTime dateUtc = DateTime.UtcNow;
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime dateNow = TimeZoneInfo.ConvertTime(dateUtc, timeZoneInfo);
            Box box = _context.Boxes.FirstOrDefault(b => b.MacAddress == device);
            if (box == null)
            {
                returnList.Add("Box not found");
                return returnList;
            }
            List<string> sigfoxData = Helpers.Hexhelper.HexConv(callback);
            //configuratie
            if (sigfoxData[0] == "1")
            {
                sigfoxData.Remove("1");
                sigfoxData.RemoveAll(p => p == "0");
                List<string> dummyData = new List<string>();
                foreach (string sensor in sigfoxData)
                {
                    if (await _context.Sensors.FindAsync(int.Parse(sensor)) != null)
                    {
                        if (await _context.SensorBoxes.FindAsync(box.BoxID, int.Parse(sensor)) == null)
                        {
                            SensorBox sensorbox = new SensorBox();
                            sensorbox.BoxID = box.BoxID;
                            sensorbox.SensorID = int.Parse(sensor);
                            _context.SensorBoxes.Add(sensorbox);
                        }
                        returnList.Add("Sensor with id: " + sensor + " was added to box: "+box.BoxID);
                        dummyData.Add(sensor);
                    }
                    else
                    {
                        returnList.Add("Sensor with id: "+sensor+" does not exist in the Database and could not be added");
                        //if sensor does not exist in db add sensorid+255 to configurationcall
                        dummyData.Add((int.Parse(sensor) +255).ToString());
                    }
                    
                }

                box.ConfiguratieString = string.Join(",", dummyData);
                await _context.SaveChangesAsync();
                returnList.Add(box.ConfiguratieString);
                returnList.Add("Configuratie aanvaard");
                return returnList;
            } else if ((sigfoxData[0] == "2") || (sigfoxData[0] == "4"))
            {
                //meting call
                string configuratieString = box.ConfiguratieString;
                string[] sensors = configuratieString.Split(',');
                if (sigfoxData[0] == "4")
                {
                    
                    
                    _context.Entry(box).State = EntityState.Modified;
                }
                sigfoxData.Remove(sigfoxData[0]);
                foreach (string sensor in sensors)
                {
            
                    string value = Helpers.ErrorHelper.CheckForErrors(_context, sensor, sigfoxData[0]);
                    Measurement measurement = new Measurement();
                    measurement.SensorID = int.Parse(sensor);
                    measurement.BoxID = box.BoxID;
                    measurement.TimeStamp = dateNow;
                    measurement.Value = value;
                    sigfoxData.Remove(sigfoxData[0]);
                    _context.Measurements.Add(measurement);
                    returnList.Add("Sensor "+sensor+" with measurement "+value);
                }
                await _context.SaveChangesAsync();
                //meting
                return returnList;

            }
            else if (sigfoxData[0] == "3"|| sigfoxData[0] == "5")
            {
                //snapshot
                sigfoxData.Remove(sigfoxData[0]);

                Monitoring monitoring = new Monitoring();
                monitoring.TimeStamp = dateNow;
                monitoring.BoxID = box.BoxID;
                returnList.Add("Timestamp: " + monitoring.TimeStamp);
                returnList.Add("BoxID: " + monitoring.BoxID);
                //batterij
                if (int.Parse(sigfoxData[0]) >= 0 && int.Parse(sigfoxData[0]) <= 100) {
                    monitoring.BatteryPercentage = sigfoxData[0];
                }
                else if (int.Parse(sigfoxData[0]) >100 && int.Parse(sigfoxData[0])<= 200)
                {
                    monitoring.BatteryPercentage = "Charging rate: "+ (int.Parse(sigfoxData[0])-100)+"%";
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
                else
                {
                    monitoring.BatteryPercentage = "Unknown Error";
                }
                returnList.Add("Battery: "+monitoring.BatteryPercentage);
                //SD card
                if (int.Parse(sigfoxData[1]) >= 0 || int.Parse(sigfoxData[1]) <= 100)
                {
                    monitoring.SdCapacity = (100-double.Parse(sigfoxData[1])).ToString();
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
                else
                {
                    monitoring.BatteryPercentage = "Value not in range (0-100)";
                }
                returnList.Add("SD Card: " + monitoring.SdCapacity);
                //satellite
                monitoring.AmountSatellite = sigfoxData[2];
                returnList.Add("Satellites: " + monitoring.AmountSatellite);
                //temperatuur
                monitoring.Temperature = sigfoxData[3];
                returnList.Add("Temperature: " + monitoring.Temperature);
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
                if (box.Comment != "")
                {
                    if (box.Comment.Contains("LastKnownLocation"))
                    {
                        int startIndex = box.Comment.IndexOf("LastKnownLocation");
                        int endIndex = box.Comment.IndexOf(",", startIndex);
                        box.Comment = box.Comment.Substring(0, startIndex + 18) + sigfoxData[6] + box.Comment.Substring(endIndex, box.Comment.Length - endIndex);
                    }
                    else
                    {
                        box.Comment += "LastKnownLocation=" + sigfoxData[6] + ",";
                    }
                    
                }
                else
                {
                    box.Comment = "LastKnownLocation="+sigfoxData[6]+",";
                }
                _context.Entry(box).State = EntityState.Modified;
                measurement.BoxID = box.BoxID;
                measurement.SensorID = 17;
                measurement.TimeStamp = dateNow;
                string datum = await getGoodWeatherDate(box.BoxID);
                //string datum = "2020-11-03";
                measurement.Value = sigfoxData[4] + ";" + sigfoxData[5] + ";" + datum;
                returnList.Add("GPS: " + measurement.Value);
                _context.Measurements.Add(measurement);
                _context.Monitorings.Add(monitoring);
                await _context.SaveChangesAsync();
                
                returnList.Add(string.Join(',', sigfoxData));
                return returnList;
            }
            returnList.Add("Identifier not caught");
            return returnList;
        }
        //Get good url
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
        //Get good date
        // api/Sigfox/{boxid}
        [HttpPost("{boxid}")]
        public async Task<string> getGoodWeatherDate(int boxid, double cloudFactor = 0.01,double scale = 0.1)
        {
            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == boxid).OrderBy(b => b.TimeStamp).LastAsync();
            string[] coords = measurement.Value.Split(";");
            
            double lat = double.Parse(coords[0]);
            double lon = double.Parse(coords[1]);
            List<string> result = await findGoodDateAPI(lon, lat, cloudFactor, scale);
            return result[0];
        }
        //get good date (first check db)
        // api/Sigfox/DB/{boxid}
        [HttpPost("DB/{boxid}")]
        public async Task<string[]> getDateFromDB(int boxid)
        {

            Measurement measurement = await _context.Measurements.Where(b => b.SensorID == 17 && b.BoxID == boxid).OrderBy(b => b.TimeStamp).LastOrDefaultAsync();
            string date = "";
            string[] fullCoords = new string[3];
            if (measurement != null && measurement.Value.Length > 0)
            {
                string[] coords = measurement.Value.Split(";");
                fullCoords[0] = coords[0];
                fullCoords[1] = coords[1];
                if (coords.Length == 3)
                {
                    date = coords[2];
                    fullCoords[2] = date;
                }
                else
                {
                    date = await getGoodWeatherDate(boxid);
                    
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

            return fullCoords;
        }
        private async Task<List<string>> findGoodDateAPI(double lon, double lat, double cloudfactor, double scale)
        {
            HttpClient client = new HttpClient();
            string eindatum = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string begindatum = DateTime.Now.Date.AddDays(-90).ToString("yyyy-MM-dd");
            string uri = "https://services.terrascope.be/timeseries/v1.0/ts/S2_CLOUDCOVER_GLOBAL/geometry/?startDate=" + begindatum + "&endDate=" + eindatum;
            //top left
            Position TLpos = new Position(lon - scale, lat + scale);
            //bottom left
            Position BLpos = new Position(lon - scale, lat - scale);
            //bottom right
            Position BRpos = new Position(lon + scale, lat - scale);
            //top right
            Position TRpos = new Position(lon + scale, lat + scale);
            Position[] posArray = { TLpos, BLpos, BRpos, TRpos, TLpos };
            LinearRing[] linearRing = { new LinearRing(posArray) };
            Polygon polygon = new Polygon(linearRing);

            string Json = JsonConvert.SerializeObject(polygon);
            var httpcontent = new StringContent(Json, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(uri, httpcontent);
            var responseString = await postResponse.Content.ReadAsStringAsync();
            dynamic responseBody = JsonConvert.DeserializeObject(responseString);
            dynamic s2Data = responseBody["results"];
            Console.WriteLine(s2Data);
            string highDate = "";
            double highData = 0;
            foreach (dynamic valuePerJsonDate in responseBody["results"])
            {
                string date = valuePerJsonDate["date"];
                double data = valuePerJsonDate["result"]["average"];
                if (data < cloudfactor)
                {
                    highData = data;
                    highDate = date;
                }
            }
            List<string> returnList = new List<string>();
            if (highDate != "")
            {
          
                returnList.Add(highDate);
                returnList.Add(highData.ToString());

            }
            else
            {
                returnList.Add("No good data found in the last 45 days");
            }

            return returnList;

        }
    }
}


