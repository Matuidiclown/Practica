using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Models.Entities
{
    public class BookingDetail : Entity
    {
        public int BookingHeaderId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
       
    }
}
