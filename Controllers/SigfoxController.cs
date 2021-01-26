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
        
        //POST: api/Sigfox/PostData
        [HttpPost("PostData")]
        public async Task PostSigfoxData(string callback)
        {
            string[] items = callback.Split(',');
            Box box = _context.Boxes.FirstOrDefault(b => b.MacAdress == items[0]);
            if (box==null)
            {
                return;
            }
            List<string> sigfoxData = Helpers.Hexhelper.HexConv(items[1]);
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
            } else if (sigfoxData[0] == "2")
            {
                string configuratieString = box.ConfiguratieString;
                string[] sensors = configuratieString.Split(',');
                sigfoxData.Remove("2");
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
            }
            else if (sigfoxData[0] == "3")
            {
                sigfoxData.Remove("3");
                //snapshot
            }
                return ;
        }

        
    }
}


