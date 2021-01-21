using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Sensor
    {
        public int SensorID { get; set; }
        public string Name { get; set; }
        public string UniqueID { get; set; }
        //relations
        public int SensorTypeID { get; set; }
        public SensorType SensorType { get; set; }
        public ICollection<SensorBox> SensorBoxes { get; set; }
    }
}
