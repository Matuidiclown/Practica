using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Models.Entities
{
    public class BookingHeader : Entity
    {
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
       

        public List<BookingDetail> Details { get; set; } = new List<BookingDetail>();
    }
}