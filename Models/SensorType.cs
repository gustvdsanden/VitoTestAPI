using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class SensorType
    {
        public int SensorTypeID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        [NotMapped]
        public ICollection<Sensor> Sensors { get; set; }
    }
}
