using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class SensorBox
    {
        public int BoxID { get; set; }
        public Box Box { get; set; }
        public int SensorID { get; set;}
        public Sensor Sensor { get; set; }
    }
}
