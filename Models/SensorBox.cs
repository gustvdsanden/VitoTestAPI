using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class SensorBox
    {
        //Relations

        public int BoxID { get; set; }
        public Box Box { get; set; }
        public int SensorID { get; set; }
        public Sensor Sensor { get; set; }

        [NotMapped]
        public ICollection<Measurement> Measurements { get; set; }
    }
}
