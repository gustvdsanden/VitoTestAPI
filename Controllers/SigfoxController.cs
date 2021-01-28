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
        public async Task<string> PostSigfoxData(string callback,string device)
        {

            Box box = _context.Boxes.FirstOrDefault(b => b.MacAddress == device);
            if (box==null)
            {
                return "Box not found";
            }
            List<string> sigfoxData = Helpers.Hexhelper.HexConv(callback);
            if (sigfoxData[0]== "1")
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
                    box.Comment ="Geen Gps beschikbaar";
                    _context.Entry(box).State = EntityState.Modified;
                }
                sigfoxData.Remove(sigfoxData[0]);
                foreach(string sensor in sensors)
                {
                    if(sensor != "0")
                    {
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
                //batterij
                Monitoring monitoring = new Monitoring();
                if (int.Parse(sigfoxData[0]) >= 0 || int.Parse(sigfoxData[0]) <= 100){
                    monitoring.BatteryPercentage = sigfoxData[0];
                }
                else if (sigfoxData[0]=="253")
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
                if (await _context.SensorBoxes.FindAsync(box.BoxID, 18) == null)
                {
                    SensorBox sensorbox = new SensorBox();
                    sensorbox.BoxID = box.BoxID;
                    sensorbox.SensorID = 18;
                    _context.SensorBoxes.Add(sensorbox);
                }
                measurement.BoxID = box.BoxID;
                measurement.SensorID = 18;
                measurement.Value = sigfoxData[6];
                measurement.TimeStamp = DateTime.Now;
                _context.Measurements.Add(measurement);
                _context.Monitorings.Add(monitoring);
                await _context.SaveChangesAsync();
                return string.Join(',', sigfoxData);
            }
                return "Identifier not caught";
        }

        
    }
}


