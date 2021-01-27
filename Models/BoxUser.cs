using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoTestAPI.Models
{
    public class BoxUser
    {
        public int BoxUserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //Relations
        public int BoxID { get; set; }
        public Box Box { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
