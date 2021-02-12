using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Box
    {
        public int BoxID { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string ConfiguratieString { get; set; }
        public bool? Active { get; set; }
        public ICollection<SensorBox> SensorBoxes { get; set; }
        public ICollection<Monitoring> Monitorings { get; set; }
    }
}
