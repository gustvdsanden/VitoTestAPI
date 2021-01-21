using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //relations
        public int BoxUserID { get; set; }
        public BoxUser BoxUser {get; set;}
    }
}
