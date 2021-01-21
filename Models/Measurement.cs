﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Measurement
    {
        public int MeasurementID { get; set; }
        public DateTime DateTime { get; set; }
        public string Value { get; set; }
        
        //relations
        public int BoxID { get; set; }
        public Box Box { get; set; }
        public int SensorID { get; set; }
        public Sensor Sensor { get; set; }
    }
}
