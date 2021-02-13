using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Sensor
    {
        public int SensorID { get; set; }
        public string Name { get; set; }
        //relations
        public int SensorTypeID { get; set; }
        public SensorType SensorType { get; set; }

        [NotMapped]
        public string Unit { get; set; }
    }
}
