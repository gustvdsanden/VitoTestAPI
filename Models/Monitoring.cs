using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Monitoring
    {
        public int MonitoringID { get; set; }
        public DateTime DateTime { get; set; }
        public string SdCapacity { get; set; }
        public string BatteryPercentage { get; set; }
        public bool BatteryStatus { get; set; }
        //Relations
        public int BoxID { get; set; }
        public Box Box { get; set; }
    }
}
