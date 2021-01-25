using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Measurement
    {
        public int MeasurementID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        
        //relations
        public int BoxID { get; set; }
        public int SensorID { get; set; }
        public SensorBox SensorBox { get; set; }
    }
}
